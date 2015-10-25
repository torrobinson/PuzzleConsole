using PuzzleConsole.ActorTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.Timers;

namespace PuzzleConsole.Game
{
    public class GameInstance
    {
        public int TicksPerSecond = 20;
        public List<ActorLayer> Layers;
        public Player Player;
        public Viewport view;
        public int TickCount;

        private System.Threading.Timer clock;
        private bool paused = false;
        private ActorLayer pausedLayer;

        public GameInstance() {
        
        }

        public void RenderAndInput() {

            //Loop and wait for input
            while (true)
            {
                //Set the viewports camera location to the player
                view.CameraLocation = Player.Location;

                //Render the frame using the layers we've defined
                view.RenderFrame(Layers);

                //Normal game input
                if (!paused && Console.KeyAvailable)
                {
                    //Pause and capture movements
                    switch (Console.ReadKey(false).Key)
                    {
                        case ConsoleKey.NumPad8:
                            Player.Move(Common.Direction.Up);
                            break;
                        case ConsoleKey.NumPad2:
                            Player.Move(Common.Direction.Down);
                            break;
                        case ConsoleKey.NumPad5:
                            Player.Move(Common.Direction.Down);
                            break;
                        case ConsoleKey.NumPad4:
                            Player.Move(Common.Direction.Left);
                            break;
                        case ConsoleKey.NumPad6:
                            Player.Move(Common.Direction.Right);
                            break;

                        case ConsoleKey.NumPad7:
                            Player.Move(Common.Direction.UpLeft);
                            break;
                        case ConsoleKey.NumPad9:
                            Player.Move(Common.Direction.UpRight);
                            break;
                        case ConsoleKey.NumPad1:
                            Player.Move(Common.Direction.DownLeft);
                            break;
                        case ConsoleKey.NumPad3:
                            Player.Move(Common.Direction.DownRight);
                            break;



                        case ConsoleKey.Escape:
                            Pause();
                            break;
                    }
                }

                //Pause menu input
                if (paused && Console.KeyAvailable)
                {
                    //Pause and capture movements
                    switch (Console.ReadKey(false).Key)
                    {
                        case ConsoleKey.Escape:
                            Resume();
                            break;
                    }
                }
                //Cancel out any user text entered
                Console.Write("\b \b");
            }
        }

        public void Initialize() {
            if (clock == null) {
                initializeClock();
            }

            //Load in some layers of actors
            ActorLayer foreground = new ActorLayer("Foreground stuff", 1, (GameInstance)this);
            foreground.InitializeFromFile("Maps/foreground.txt");

            ActorLayer wallsAndItems = new ActorLayer("Main layer with players, walls, and items", 0, (GameInstance)this);
            wallsAndItems.InitializeFromFile("Maps/walls_and_items.txt");

            ActorLayer background = new ActorLayer("Background stuff", -1, (GameInstance)this);
            background.InitializeFromFile("Maps/background.txt");

            //initialize the pause menu/layer
            pausedLayer = new ActorLayer("Paused layer", -999, (GameInstance)this);
            pausedLayer.InitializeFromFile("Maps/paused.txt");
            pausedLayer.AlwaysCentered = true;

            Layers = new List<ActorLayer>() { foreground, wallsAndItems, background };
            Player = (Player)wallsAndItems.FindFirstObjectInWorldOfType(typeof(Player));

            //Create a viewport sized for this map
            view = new Viewport(25, 30);
            view.CameraLocation = Player.Location;

            //Read user keyboard input
            RenderAndInput();
        }

        public void Pause() {
            Layers.Add(pausedLayer);
            clock.Change(Timeout.Infinite, Timeout.Infinite);
            paused = true;
        }

        public void Resume() {
            Layers.Remove(pausedLayer);
            initializeClock();
            paused = false;

        }

        private void initializeClock() {
            TimerCallback tcb = Tick;
            clock = new System.Threading.Timer(tcb, null, 0, 1000 / TicksPerSecond);
        }

        public static event EventHandler TickHandler;
        public void Tick(object state)
        {
            TickCount++;
            var tickHandler = TickHandler;
            if (tickHandler != null)
                tickHandler(this, new EventArgs());
        }

        
    }
}

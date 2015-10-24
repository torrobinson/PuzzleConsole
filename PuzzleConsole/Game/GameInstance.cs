using PuzzleConsole.ActorTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PuzzleConsole.Game
{
    public class GameInstance
    {
        public int TicksPerSecond = 20;
        public List<ActorLayer> Layers;
        public Player Player;
        public Viewport view;

        private Timer clock;
        private int tickCount;
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
                        case ConsoleKey.UpArrow:
                            Player.Move(Common.Direction.Up);
                            break;
                        case ConsoleKey.DownArrow:
                            Player.Move(Common.Direction.Down);
                            break;
                        case ConsoleKey.LeftArrow:
                            Player.Move(Common.Direction.Left);
                            break;
                        case ConsoleKey.RightArrow:
                            Player.Move(Common.Direction.Right);
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
            pausedLayer = new ActorLayer("Paused layer", -99999, (GameInstance)this);
            pausedLayer.InitializeFromFile("Maps/paused.txt");
            pausedLayer.AlwaysCentered = true;

            Layers = new List<ActorLayer>() { foreground, wallsAndItems, background };
            Player = (Player)wallsAndItems.FindFirstObjectInWorldOfType(typeof(Player));

            //Create a viewport sized for this map
            //Viewport view = new Viewport(wallsAndItems.Height, wallsAndItems.Width);
            view = new Viewport(20, 30);

            //Start the game clock
            clock.Start();

            RenderAndInput();
        }

        public void Stop() {
            //Stop the game clock
            clock.Stop();
        }

        public void Pause() {
            Layers.Add(pausedLayer);
            clock.Stop();
            paused = true;
        }

        public void Resume() {
            Layers.Remove(pausedLayer);
            clock.Start();
            paused = false;

        }

        private void initializeClock() {
            clock = new Timer(1000/TicksPerSecond);
            clock.Elapsed += Tick(new EventArgs());
        }

        public static event EventHandler TickHandler;
        public ElapsedEventHandler Tick(EventArgs args)
        {
            //Send the Tick event to all subscribers

            tickCount++;

            var tickHandler = TickHandler;
            if (tickHandler != null)
                tickHandler(this, args);

            return null;
        }

        
    }
}

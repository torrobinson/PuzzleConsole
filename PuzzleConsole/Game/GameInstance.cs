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
        public int FramesPerSecond = 90;
        private object frameSync = new object();

        public List<ActorLayer> Layers;
        public Player Player;
        public Viewport view;
        private ActorLayer pausedLayer;
        public static event EventHandler TickHandler;

        private System.Threading.Timer gameClock;
        private System.Threading.Timer frameClock;
        public int TickCount;

        private bool paused = false;
        
        public GameInstance() {
        
        }

        //Awaits user input on this thread
        public void ReadInput() {

            //Loop and wait for input
            while (true)
            {
                //Normal game input
                if (!paused && Console.KeyAvailable)
                {
                    //Pause and capture movements
                    switch (Console.ReadKey(true).Key)
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
                            TogglePause();
                            break;
                    }
                }

                //Paused state input
                if (paused && Console.KeyAvailable)
                {
                    //Pause and capture movements
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Escape:
                            TogglePause();
                            break;
                    }
                }
            }
        }




        //Starts a game
        public void Initialize() {

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
            pausedLayer.Visible = false;

            Layers = new List<ActorLayer>() {pausedLayer, foreground, wallsAndItems, background };
            Player = (Player)wallsAndItems.FindFirstObjectInWorldOfType(typeof(Player));

            //Create a viewport sized for this map
            view = new Viewport(25, 30);
            view.CameraLocation = Player.Location;

            //Start the game clock
            gameClock = new System.Threading.Timer(new TimerCallback(GameTick), null, 0, 1000 / TicksPerSecond);
            frameClock = new System.Threading.Timer(new TimerCallback(FrameTick), null, 0, 1000 / FramesPerSecond);

            //Draw the first frames
            FrameTick();

            //Read user keyboard input
            ReadInput();
        }


        //Pausing
        public void Pause() {
            pausedLayer.Visible = true; 
            gameClock.Change(Timeout.Infinite, Timeout.Infinite);
            frameClock.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void Resume() {
            pausedLayer.Visible = false;
            gameClock = new System.Threading.Timer(new TimerCallback(GameTick), null, 0, 1000 / TicksPerSecond);
            frameClock = new System.Threading.Timer(new TimerCallback(FrameTick), null, 0, 1000 / FramesPerSecond);
        }

        public void TogglePause() {
            if (paused) Resume();
            else Pause();
            paused = !paused;
            view.RenderFrame(Layers); //otherwise the shift to a pause state wont be rendered (since we stopped the frame clock)
        }





        //Ticking
        public void GameTick(object state = null)
        {
            TickCount++;
            var tickHandler = TickHandler;
            if (tickHandler != null)
            {
                tickHandler(this, new EventArgs());
            }
        }

        public void FrameTick(object state = null)
        {
            lock (frameSync) {
                //Set the viewports camera location to the player
                view.CameraLocation = Player.Location;

                //Render the frame using the layers we've defined
                view.RenderFrame(Layers);
            }
        }

        


    }
}

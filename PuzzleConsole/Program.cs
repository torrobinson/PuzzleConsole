using PuzzleConsole.ActorTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PuzzleConsole
{
    class Program
    {

        static void Main(string[] args)
        {
            //Prepare the 3 layers of objects/actors we want in play
            ActorLayer foreground = new ActorLayer("Foreground stuff",                            1);
            foreground.InitializeFromFile("Maps/foreground.txt");

            ActorLayer wallsAndItems = new ActorLayer("Main layer with players, walls, and items",0);
            wallsAndItems.InitializeFromFile("Maps/walls_and_items.txt");

            ActorLayer background = new ActorLayer("Background stuff",                           -1);
            background.InitializeFromFile("Maps/background.txt");

            List<ActorLayer> layersToRender = new List<ActorLayer>(){foreground, wallsAndItems, background };


            //Create a viewport sized for this map
            //Viewport view = new Viewport(wallsAndItems.Height, wallsAndItems.Width);
            Viewport view = new Viewport(20, 30);

            //Fetch the first player in the map and bind controls to them (the player was automatically added from being read in from the map)
            Player thePlayer = (Player)wallsAndItems.FindFirstObjectInWorldOfType(typeof(Player));

            //Set the viewports camera location to the player
            view.CameraLocation = thePlayer.Location;

            //Loop and wait for input
            while (true) {

                //Render the frame using the layers we've defined
                view.RenderFrame(layersToRender);
                //Pause and capture movements
                switch (Console.ReadKey(false).Key)
                {
                    case ConsoleKey.UpArrow:
                        thePlayer.Move(Common.Direction.Up);
                        break;
                    case ConsoleKey.DownArrow:
                        thePlayer.Move(Common.Direction.Down);
                        break;
                    case ConsoleKey.LeftArrow:
                        thePlayer.Move(Common.Direction.Left);
                        break;
                    case ConsoleKey.RightArrow:
                        thePlayer.Move(Common.Direction.Right);
                        break;
                }

                //After the movement, update the camera location before the next frame render
                view.CameraLocation = thePlayer.Location;

                //If the user tytped any printable characters, this will keep them from writing over the world
                // by jumping the cursor back
                Console.Write("\b \b");
            }
            
        }
    }
}

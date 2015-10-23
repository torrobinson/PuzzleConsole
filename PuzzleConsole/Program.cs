using PuzzleConsole.WorldTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //Prep world and view (of the same size)

            ActorLayer foreground = new ActorLayer(1, 1, 1);
            foreground.InitializeFromFile("foreground.txt");

            ActorLayer wallsAndItems = new ActorLayer(1,1,0);
            wallsAndItems.InitializeFromFile("walls_and_items.txt");

            ActorLayer background = new ActorLayer(1, 1,-1);
            background.InitializeFromFile("background.txt");



            List<ActorLayer> layersToRender = new List<ActorLayer>(){foreground, wallsAndItems, background };

            //Now we have a wallsAndItems layer and a background layer below

            //Create a viewport sized for this map
            Viewport view = new Viewport(wallsAndItems.Height, wallsAndItems.Width);

            //Fetch the first player in the map and bind controls to them (the player was automatically added from being read in from the map)
            Player thePlayer = (Player)wallsAndItems.FindFirstObjectInWorldOfType(typeof(Player));
            
            //Loop forever and ever
            while (true) {
                //Render the "frame"
                view.RenderWorld(layersToRender);

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

                System.Threading.Thread.Sleep(10);
            }
            
        }
    }
}

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
            World world = new World(1,1);
            world.InitializeFromFile("map.txt");
            Viewport view = new Viewport(world.height, world.width);

            //Fetch teh first player in the map and bind controls to them
            Player thePlayer = (Player)world.FindFirstObjectInWorldOfType(typeof(Player));
            
            //Loop forever and ever
            while (true) {
                //Render the "frame"
                view.RenderWorld(world);

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

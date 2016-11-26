using PuzzleConsole.ActorTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole.Algorithms
{
    public static class PathFinderHelpers
    {
        public static byte[,] createByteGridFromLayer(ActorLayer layer) {
            byte[,] grid = new byte[layer.Width, layer.Height];

            int x = 0, y = 0;
            foreach(List<Actor> row in layer.Actors){
                foreach(Actor actor in row){
                    grid[x,y] = Convert.ToByte(actor.Clippable);
                    x++;
                }
                y++;
                x = 0;
            }
            return grid;
        }
    }
}

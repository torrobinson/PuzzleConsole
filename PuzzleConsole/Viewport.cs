using PuzzleConsole.WorldTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole
{
    public class Viewport
    {
        public int Height;
        public int Width;

        public Viewport(int height, int width) {
            this.Height = height;
            this.Width = width;
            refreshSize();
        }

        public void RenderWorld(List<ActorLayer> layers){
            int x, y;
            Console.Clear();
            //For each visible layer from back to front
            foreach(ActorLayer layer in layers.OrderBy(l => l.ZIndex).Where(l => l.Visible)){
                x = 0;
                y = 0;
                //for each row of actors on that layer
                foreach(List<Actor> row in layer.Actors){

                    //for each actor in that row
                    foreach(Actor actor in row){
                        if (actor != null)
                        {
                            Console.ForegroundColor = actor.color;
                            Console.SetCursorPosition(x, y);
                            Console.Write(actor.ToString());
                        }
                        else {
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        x++;
                    }
                    x = 0;
                    y++;
                }
            }
        }

        private void refreshSize() {
            Console.SetWindowSize(Width, Height);
        }

    }
}

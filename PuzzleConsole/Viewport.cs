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

        public void RenderWorld(World world){
            Console.Clear();
            Console.CursorVisible = false;

            //Render all world items
            foreach(List<WorldObject> row in world.Objects){
                foreach (WorldObject worldObject in row) {
                    if(worldObject == null 
                        || (
                            worldObject != null && !worldObject.Visible
                        )){
                        Console.Write(" ");     //display as nothing if there's nothing here or the object isn't visible
                    }
                    else{
                        Console.Write(worldObject.ToString());
                    }
                }
                Console.WriteLine();
            }
        }

        private void refreshSize() {
            Console.SetWindowSize(Width, Height);
        }

    }
}

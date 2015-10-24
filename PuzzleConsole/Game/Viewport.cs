using PuzzleConsole.ActorTypes;
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
        private List<List<Actor>> currentFrame = null;
        private List<List<Actor>> previousFrame = null;

        public Viewport(int height, int width) {
            this.Height = height;
            this.Width = width;
            refreshSize();
        }

        public void RenderFrame(List<ActorLayer> layers){
            int x = 0 ;
            int y = 0 ;
            List<List<Actor>> deltaToDraw;

            //Only worry about those that are visible
            layers = layers.Where(l => l.Visible).ToList();

            //First create the holder for the current frame. Height and width will be the maxes for all layers to be rendered
            currentFrame = Common.CreateLayer(
                    layers.Select(l => l.Height).Max(),
                    layers.Select(l => l.Width).Max(),
                    typeof(Empty)
            );

            //Going bottom-to-top, combine all the layers by overwriting them ontop of eachother
            foreach (ActorLayer layer in layers.OrderBy(l => l.ZIndex))
            {
                x = 0;
                y = 0;
                foreach (List<Actor> row in layer.Actors)
                {

                    //for each actor in that row
                    foreach (Actor actor in row)
                    {
                        //Render if the actor exists and there's nothing above it (no need to render if we can't see it)
                        if (actor.GetType() != typeof(Empty))
                        {
                            currentFrame[y][x] = actor;
                        }
                        x++;
                    }
                    x = 0;
                    y++;
                }
            }

            //Then compare this combined frame with the previous frame to come up with a delta to draw
            if (previousFrame != null)
            {
                //Initialize the delta to empty, of the same size as the currentframe
                deltaToDraw = Common.CreateLayer(
                    currentFrame.Count, 
                    currentFrame.Select(f => f.Count).Max()
                    );

                //For each currentFrame cell, if the previous frame isn't that value, then set the delta to be the currentFrame cell value
                y = 0;
                x = 0;
                foreach(List<Actor> currentFrameRow in currentFrame){

                    foreach(Actor currentFrameActor in currentFrameRow){
                        //If there was a change
                        if (previousFrame[y][x].ToString() != currentFrameActor.ToString())
                        {
                            deltaToDraw[y][x] = currentFrameActor;
                        }
                        x++;
                    }
                    x=0;
                    y++;
                }
            }
            else {
                //previous frame was null, so the delta IS the currentFrame
                deltaToDraw = currentFrame;
            }

            //Then draw only the delta 
            //for each row of actors on that layer
            x = 0;
            y = 0;
            foreach (List<Actor> deltaRow in deltaToDraw)
            {
                //for each actor in that row
                foreach (Actor delta in deltaRow)
                {
                    //Render if the change exists
                    if (delta != null)
                    {
                        WriteStringToConsoleAtPosition(delta.ToString(), x, y, delta.color);
                    }
                    x++;
                }
                x = 0;
                y++;
            }

            //Set the previous/last frame drawn to the one we just drew
            previousFrame = currentFrame;
        }

        protected static void WriteStringToConsoleAtPosition(string str, int x, int y, ConsoleColor color = ConsoleColor.White)
        {
            try
            {
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = color;
                Console.Write(str);
                Console.SetCursorPosition(0, 0); //otherwise characters surrounding the crrent position will be overwritten with the key the user pressed!
            }
            catch (ArgumentOutOfRangeException e){}
        }

        private void refreshSize() {
            Console.SetWindowSize(Width, Height);
            Console.CursorVisible = false;
        }

    }
}

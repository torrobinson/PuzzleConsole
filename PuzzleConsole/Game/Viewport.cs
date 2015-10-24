using PuzzleConsole.ActorTypes;
using PuzzleConsole.Game;
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
        public Point CameraLocation;
        private List<List<Actor>> currentFrame = null;
        private List<List<Actor>> previousFrame = null;

        public Viewport(int height, int width) {
            this.Height = height;
            this.Width = width;
            RefreshSize();
        }

        public void RenderFrame(List<ActorLayer> layers){

            //Create a new list of layers, cloned and cropped, to draw
            List<ActorLayer> layersToDraw = new List<ActorLayer>();
            foreach (ActorLayer layer in layers.Where(l=>l.Visible)) {
                layersToDraw.Add(layer.Clone());
            }

            //Cookie-cut all layers into the viewport, based on size and camera location
            List<ActorLayer> cutLayers = new List<ActorLayer>();
            foreach (ActorLayer layer in layersToDraw)
            {
                ActorLayer cutLayer = Common.CutLayerToSize(layer, CameraLocation, Width, Height);
                cutLayers.Add(cutLayer);
            }

            //The frame to draw to the screen
            List<List<Actor>> frameToDraw;

            //Merge the layers down into a frame
            currentFrame = MergeLayers(layersToDraw);

            //Then compare this combined frame with the previous frame to come up with a delta to draw
            if (previousFrame != null)
            {
                frameToDraw = FindLayerDelta(previousFrame, currentFrame);
            }
            else {
                //previous frame was null, so the delta IS the currentFrame
                frameToDraw = currentFrame;
            }

            //Draw changes to screen
            DrawFrameToConsole(frameToDraw);

            //Store the successfully rendered frame for next time
            previousFrame = currentFrame;

            //
            layersToDraw = null;
        }

        protected static void WriteStringToConsoleAtPosition(string str, int x, int y, ConsoleColor foreColor = ConsoleColor.White, ConsoleColor backColor = ConsoleColor.Black)
        {
            try
            {
                Console.SetCursorPosition(x, y);
                Console.BackgroundColor = backColor;
                Console.ForegroundColor = foreColor;
                Console.Write(str);
                Console.SetCursorPosition(0, 0); //otherwise characters surrounding the crrent position will be overwritten with the key the user pressed!
            }
            catch (ArgumentOutOfRangeException e){}
        }

        public void RefreshSize() {
            Console.SetWindowSize(Width, Height);
            Console.CursorVisible = false;
        }

        private List<List<Actor>> FindLayerDelta(List<List<Actor>> before, List<List<Actor>> after) {
            //Initialize the delta to empty, of the same size as the currentframe
            List<List<Actor>> delta = Common.CreateLayer(
                currentFrame.Count,
                currentFrame.Select(f => f.Count).Max()
                );

            //For each currentFrame cell, if the previous frame isn't that value, then set the delta to be the currentFrame cell value
            int y = 0;
            int x = 0;
            foreach (List<Actor> currentFrameRow in currentFrame)
            {

                foreach (Actor currentFrameActor in currentFrameRow)
                {
                    //If there was a change
                    if (previousFrame[y][x].ToString() != currentFrameActor.ToString())
                    {
                        delta[y][x] = currentFrameActor;
                    }
                    x++;
                }
                x = 0;
                y++;
            }

            return delta;
        }

        public void DrawFrameToConsole(List<List<Actor>> frame) {
            int x = 0;
            int y = 0;
            foreach (List<Actor> deltaRow in frame)
            {
                //for each actor in that row
                foreach (Actor delta in deltaRow)
                {
                    //Render if the change exists
                    if (delta != null)
                    {
                        WriteStringToConsoleAtPosition(delta.ToString(), x, y, delta.foreColor, delta.backColor);
                    }
                    x++;
                }
                x = 0;
                y++;
            }
        }

        private List<List<Actor>> MergeLayers(List<ActorLayer> layers)
        {
            //First create the holder for the current frame. Height and width will be the maxes for all layers to be rendered
            List<List<Actor>> merged =  Common.CreateLayer(
                    layers.Select(l => l.Height).Max(),
                    layers.Select(l => l.Width).Max(),
                    typeof(Empty)
            );
            int x, y;
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
                        //Render if the actor exists and isnt an empty type (theres something to render)
                        if (actor != null && actor.GetType() != typeof(ActorTypes.Empty) && !(actor.GetType().IsSubclassOf(typeof(ActorTypes.Empty))))
                        {
                            merged[y][x] = actor;
                        }
                        x++;
                    }
                    x = 0;
                    y++;
                }
            }

            return merged;
        }

    }
}

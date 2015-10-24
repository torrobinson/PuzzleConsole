using System;
using PuzzleConsole.Game;
using PuzzleConsole.ActorTypes;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PuzzleConsole
{
	public static class Common
	{
		public enum Direction
		{
			Up,
			Down,
			Left,
			Right
		};

        public static List<List<Actor>> CreateLayer(int height, int width, Type defaultType = null)
        {
            List<List<Actor>> layer = new List<List<Actor>>();

            for(int y = 0; y < height; y++){
                List<Actor> row = new List<Actor>();

                for(int x = 0; x < width; x++){
                    row.Add(
                        defaultType == null ? null : (Actor)Activator.CreateInstance(defaultType)
                        );
                }

                layer.Add(row);
            }

            return layer;
        }

		public static Point DirectionToPointOffset(Direction direction){
			int offsetx = 0;
			int offsety = 0;

			switch (direction) {

			case PuzzleConsole.Common.Direction.Up:
				offsetx = 0;
				offsety = -1;	
				break;

			case PuzzleConsole.Common.Direction.Down:
				offsetx = 0;
				offsety = 1;	
				break;

			case PuzzleConsole.Common.Direction.Left:
				offsetx = -1;
				offsety = 0;	
				break;

			case PuzzleConsole.Common.Direction.Right:
				offsetx = 1;
				offsety = 0;	
				break;

			default:
				break;
			}

			return new Point (
				offsetx,
				offsety
			);

		}

        public static ActorLayer CutLayerToSize(ActorLayer layer, Point center, int width, int height)
        {
           //Copy the original to a new layer
           ActorLayer original = layer.Clone();

           layer.Width = width;
           layer.Height = height;

           //Set the layer to the new cut size
           layer.Actors = CreateLayer(height, width);

            //Start going through and writing the trimmed cells from the 
            //  original to the trimmed layer passed in
            int trimmedXToWrite = 0;
            int trimmedYToWrite = 0;

            int topPosition = center.Y -Convert.ToInt32(Math.Floor((decimal)height/2));
            int bottomPosition = center.Y + Convert.ToInt32(Math.Ceiling((decimal)height / 2));
            int leftPosition = center.X - Convert.ToInt32(Math.Floor((decimal)width / 2));
            int rightPosition = center.X + Convert.ToInt32(Math.Ceiling((decimal)width / 2));


            for(int y = topPosition;  y < bottomPosition; y++){
                    for (int x = leftPosition; x < rightPosition; x++){
                        if (x >= 0 && x < original.Width && y >= 0 && y < original.Height)
                        {
                            layer.Actors[trimmedYToWrite][trimmedXToWrite] = original.Actors[y][x];
                        }
                        else
                        {
                            layer.Actors[trimmedYToWrite][trimmedXToWrite] = new OutOfBounds();
                        }
                        
                        trimmedXToWrite++;

                        }

                    trimmedXToWrite = 0;
                    trimmedYToWrite++;
            }
            return layer;
        }
	}
}


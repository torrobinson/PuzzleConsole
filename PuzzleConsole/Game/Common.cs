using System;
using PuzzleConsole.Game;
using PuzzleConsole.ActorTypes;
using System.Collections.Generic;

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

            for(int y = 0; y <= height; y++){
                List<Actor> row = new List<Actor>();

                for(int x = 0; x <= width; x++){
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
	}
}


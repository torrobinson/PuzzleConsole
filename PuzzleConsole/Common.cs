using System;
using PuzzleConsole.Game;

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


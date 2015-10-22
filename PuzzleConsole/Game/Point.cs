using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole.Game
{
    public class Point
    {
        public int X;
        public int Y;

        public Point(int x, int y) {
            this.X = x;
            this.Y = y;
        }

        public Point Add(Point anotherPoint) {
            return new Point(
                    this.X + anotherPoint.X,
                    this.Y + anotherPoint.Y
                );
        }
    }
}

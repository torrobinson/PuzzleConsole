using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PuzzleConsole.Game;

namespace PuzzleConsole.ActorTypes
{
    public abstract class Movable : Actor
    {
        //Indicates that this item can move
        public bool Static = false;

        public Common.Direction Direction = Common.Direction.Down;

        public Movable()
        {

        }

        //Helper for checking if this object can move in any given directions
		public bool canMove(PuzzleConsole.Common.Direction inDirection){

            //First check if movement is diagonal - because if it is, the 2 blocks making nup the digonal movement need to be open first
            bool diagonalPossible = true;
            if (Common.GetDiagonalDirections().Contains(inDirection))
            {
                if (inDirection == Common.Direction.UpLeft)
                    diagonalPossible =  GetObjectInDirection(Common.Direction.Up).Clippable || GetObjectInDirection(Common.Direction.Left).Clippable;

                if (inDirection == Common.Direction.UpRight)
                    diagonalPossible = GetObjectInDirection(Common.Direction.Up).Clippable || GetObjectInDirection(Common.Direction.Right).Clippable;

                if (inDirection == Common.Direction.DownLeft)
                    diagonalPossible = GetObjectInDirection(Common.Direction.Down).Clippable || GetObjectInDirection(Common.Direction.Left).Clippable;

                if (inDirection == Common.Direction.DownRight)
                    diagonalPossible = GetObjectInDirection(Common.Direction.Down).Clippable || GetObjectInDirection(Common.Direction.Right).Clippable;
            }

            //Or just a simple caridnal direction
            Actor objectPossiblyInWay = GetObjectInDirection(inDirection);
            //if object can be moved into
            if (objectPossiblyInWay.Clippable && diagonalPossible)
            {
                return true; //allow moving to
            }
            //or it can be pushed out of the way
            if (diagonalPossible && objectPossiblyInWay != null && objectPossiblyInWay.GetType().IsSubclassOf(typeof(Pushable)) && ((Pushable)objectPossiblyInWay).canMove(inDirection))
            {
                ((Pushable)objectPossiblyInWay).Move(inDirection); //kick-off the push
                return true; //allow moving to the now-empty spot
            }
                   
            //Otherwise it can't move in that direction
			return false;
        }

		public bool Move(PuzzleConsole.Common.Direction inDirection){

            //Regardless, at least turn in the direction you try to move to
            Direction = inDirection;

            if (canMove(inDirection))
            {

                Point oldLocation = Location;

                Location = Location.Add(
                    PuzzleConsole.Common.DirectionToPointOffset(inDirection)
                    );

                //Tell the layer I'm on where I moved to
                //remove from old location
                Layer.Actors[oldLocation.Y][oldLocation.X] = new Empty();

                //insert into new location
                Layer.Actors[Location.Y][Location.X] = this;

                return true;
            }
            else {
                return false;
            }
		}
    }
}

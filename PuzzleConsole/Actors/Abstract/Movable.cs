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

            //First check if the direction is even valid.
            //Here we'll keep movement from happening diagonally if botht he diagonal's cardinal directions are full
            bool movementInDirectionPossible = true;
            if (Common.GetDiagonalDirections().Contains(inDirection))
            {
                if (inDirection == Common.Direction.UpLeft)
                    movementInDirectionPossible = GetObjectInDirection(Common.Direction.Up).Clippable || GetObjectInDirection(Common.Direction.Left).Clippable;

                if (inDirection == Common.Direction.UpRight)
                    movementInDirectionPossible = GetObjectInDirection(Common.Direction.Up).Clippable || GetObjectInDirection(Common.Direction.Right).Clippable;

                if (inDirection == Common.Direction.DownLeft)
                    movementInDirectionPossible = GetObjectInDirection(Common.Direction.Down).Clippable || GetObjectInDirection(Common.Direction.Left).Clippable;

                if (inDirection == Common.Direction.DownRight)
                    movementInDirectionPossible = GetObjectInDirection(Common.Direction.Down).Clippable || GetObjectInDirection(Common.Direction.Right).Clippable;
            }

            //Before bothering to check for pieces in the way, ensure that the direction is valid
            if (movementInDirectionPossible) {
                
                //Then check if the target location is free to move to.
                //Either it's free or the object there will be pushed (making it free)

                Actor objectPossiblyInWay = GetObjectInDirection(inDirection);

                //If there's a null there (shoudlnt happen because we should have an Empty clippable piece here)
                if (objectPossiblyInWay == null)
                {
                    PushedActorLastMove = null;
                    return true;
                }

                //if object can be moved into (including if there's just an empty piece here)
                if (objectPossiblyInWay.Clippable)
                {
                    PushedActorLastMove = null;
                    return true; //allow moving to
                }

                //or it can be pushed out of the way
                if (movementInDirectionPossible && objectPossiblyInWay.IsPushable() && ((Pushable)objectPossiblyInWay).canMove(inDirection))
                {
                    ((Pushable)objectPossiblyInWay).Move(inDirection); //kick-off the push
                    PushedActorLastMove = objectPossiblyInWay; //store which actor we just pushed
                    return true; //allow moving to the now-empty spot
                }
            }
                   
            //Otherwise it can't move in that direction
			return false;
        }

		public bool Move(PuzzleConsole.Common.Direction inDirection){

            //First turn the actor in the desired direction
            Direction = inDirection;

            //And then move if it's possible
            if (canMove(inDirection))
            {
                Point oldLocation = Location;

                Location = Location.Add(
                    PuzzleConsole.Common.DirectionToPointOffset(inDirection)
                    );


                //Then tell the layer I'm on where I moved to
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

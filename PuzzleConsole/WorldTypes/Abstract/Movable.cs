﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PuzzleConsole.Game;

namespace PuzzleConsole.WorldTypes
{
    public abstract class Movable : Actor
    {
        //Indicates that this item can move
        public bool Static = false;

        public Movable()
        {

        }

        //Helper for checking if this object can move in any given directions
		public bool canMove(PuzzleConsole.Common.Direction inDirection){
			Actor objectPossiblyInWay = GetObjectInDirection (inDirection);

            //if object doesnt exist
            if (objectPossiblyInWay == null)
            { 
                return true; //allow moving to an empty space
            }

            //or it can be pushed out of the way
            if (objectPossiblyInWay != null && objectPossiblyInWay.GetType().IsSubclassOf(typeof(Pushable)) && ((Pushable)objectPossiblyInWay).canMove(inDirection))
            { 
                ((Pushable)objectPossiblyInWay).Move(inDirection); //kick-off the push
                return true; //allow moving to the now-empty spot
            }
                   
            //Otherwise it can't move in that direction
			return false;
        }

		public void Move(PuzzleConsole.Common.Direction inDirection){
            
			if (canMove (inDirection)) {

                Point oldLocation = Location;

                Location = Location.Add(
                    PuzzleConsole.Common.DirectionToPointOffset(inDirection)
                    );

                UpdateWorldObjectsArrayWithNewLocation(oldLocation);
			}
		}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PuzzleConsole.Game;

namespace PuzzleConsole.ActorTypes
{
    public abstract class MovesOnOwn : Movable
    {
        public double blocksPerTick = 0.1; // blocks per tick. 0.1 means 1 block per 10 ticks, at 20 ticks a second = 2 blocks per second
       
        public MovesOnOwn()
        {
            if (Represents == null) {
                base.SubscribeToTicks();
            }
            
        }

        public override void GameTick(EventArgs args)
        {
            if( Game.TickCount % (blocksPerTick * Game.TickCount) ==  0){
                Move(Common.Direction.Right);
            }
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

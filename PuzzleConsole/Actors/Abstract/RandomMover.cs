using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PuzzleConsole.Game;

namespace PuzzleConsole.ActorTypes
{
    public abstract class RandomMover : Pushable
    {
        public double speedBlocksPerTick = 0.5; // @ 20 ticks per second: 0.1 bpt = 2 bps, 0.05 bpt = 1 bps,  0.5 bpt = 10 bps
        private Random random;
        private Common.Direction lastDirection;

        public RandomMover()
        {
            if (Represents == null) {
                base.SubscribeToTicks();
                random = new Random();
            }
            
        }

        public override void GameTick(EventArgs args)
        {
            if (Convert.ToInt32(speedBlocksPerTick * Game.TickCount) == (speedBlocksPerTick * Game.TickCount))
            {
                MakeNextMove();
            }
        }

        private List<Common.Direction> movePattern = Common.GetAllDirections();

        private int moveIndex = 0;
        public void MakeNextMove() {


             //Try until you moved in a direction
             while (!Move(movePattern[random.Next(0, movePattern.Count)])) { }

           // lastDirection = direction;
        }
    }
}

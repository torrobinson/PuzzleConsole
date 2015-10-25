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
        public double speedBlocksPerTick = 0.5; //0.1 bpt = 1 bp 10 t @ 20 tps. = 2 BPS.    0.05 = 1 BPS    0.5=10 times a second
        private Random random;

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
            while (!Move(movePattern[random.Next(0, movePattern.Count)])) {
            }
        }
    }
}

using PuzzleConsole.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole.ActorTypes
{
    [DefaultCharacterRepresentation(" ")]
    public class Empty : Actor
    {
        public Empty()
        {
            base.Clippable = true;
        }

        public double moveActionsPerTick = 0.00;
        public override double MoveActionsPerTick
        {
            get
            {
                return moveActionsPerTick;
            }
        }
    }
}

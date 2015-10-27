using PuzzleConsole.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole.ActorTypes
{
    [DefaultCharacterRepresentation(".")]
    public class Pebble : Pushable
    {
        public double moveActionsPerTick = 0.05; //1 block per sec
        public override double MoveActionsPerTick
        {
            get
            {
                return moveActionsPerTick;
            }
        }
        public Pebble()
        {
            base.foreColor = ConsoleColor.Yellow;
        }
    }
}

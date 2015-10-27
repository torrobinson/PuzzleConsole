using PuzzleConsole.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole.ActorTypes
{
    [DefaultCharacterRepresentation("+")]
    public class Player: Pushable
    {
        public Player() {
            base.foreColor = ConsoleColor.Red;
            base.backColor = ConsoleColor.Yellow;
        }

        //TODO: implement player speed into ticks
        public double moveActionsPerTick = 0.05;
        public override double MoveActionsPerTick
        {
            get
            {
                return moveActionsPerTick;
            }
        }
    }
}

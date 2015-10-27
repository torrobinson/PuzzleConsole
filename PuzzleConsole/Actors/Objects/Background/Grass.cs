using PuzzleConsole.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole.ActorTypes
{
    [DefaultCharacterRepresentation("░")]
    public class Grass:Actor
    {
        public double moveActionsPerTick = 0.05; //1 block per sec
        public override double MoveActionsPerTick
        {
            get
            {
                return moveActionsPerTick;
            }
        }

        public Grass()
        {
            base.backColor = ConsoleColor.DarkGreen;
            base.foreColor = ConsoleColor.Green;
        }

        public void GameTick(EventArgs args)
        {
            base.GameTick(args);
        }
    }
}

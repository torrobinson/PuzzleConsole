using PuzzleConsole.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole.ActorTypes
{
    public class Metal : Actor
    {
        public Metal()
        {
            base.foreColor = ConsoleColor.Black;
            base.backColor = ConsoleColor.DarkGray;
        }

        public override string CharacterRepresentation
        {
            get
            {
                return "X";
            }
        }
    }
}

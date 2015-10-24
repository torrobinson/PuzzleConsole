using PuzzleConsole.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole.ActorTypes
{
    public class Stone : Pushable
    {
        public Stone()
        {
            base.color = ConsoleColor.Magenta;
        }

        public override string CharacterRepresentation
        {
            get
            {
                return "@";
            }
        }
    }
}

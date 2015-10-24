using PuzzleConsole.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole.ActorTypes
{
    public class Player: Movable
    {
        public Player() {
            base.color = ConsoleColor.Red;
        }

        public override string CharacterRepresentation
        {
            get
            {
               return "M";
            }
        }
    }
}

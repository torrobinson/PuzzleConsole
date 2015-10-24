using PuzzleConsole.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole.ActorTypes
{
    public class Wall : Actor
    {
        public Wall() {
            base.characterRepresentation = "█";
            base.color = ConsoleColor.DarkGray;
        }
    }
}

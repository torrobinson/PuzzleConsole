using PuzzleConsole.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole.WorldTypes
{
    public class Grass:Actor
    {
        public Grass()
        {
            base.characterRepresentation = "░";
            base.color = ConsoleColor.DarkGreen;
        }
    }
}

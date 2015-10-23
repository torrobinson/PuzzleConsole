using PuzzleConsole.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole.WorldTypes
{
    public class Pebble : Pushable
    {
        public Pebble()
        {
            base.characterRepresentation = ".";
        }
    }
}

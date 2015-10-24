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
        public Pebble()
        {
            base.foreColor = ConsoleColor.Yellow;
        }

        public override void GameTick(EventArgs args)
        {

        }
    }
}

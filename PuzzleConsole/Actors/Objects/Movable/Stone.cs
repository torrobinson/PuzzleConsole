using PuzzleConsole.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole.ActorTypes
{
    [DefaultCharacterRepresentation("@")]
    public class Stone : Pushable
    {
        public Stone()
        {
            base.foreColor = ConsoleColor.Magenta;
        }

        public override void GameTick(EventArgs args)
        {
            
        }
    }
}

using PuzzleConsole.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole.ActorTypes
{
    [DefaultCharacterRepresentation("M")]
    public class MoveTest : Follower {
        public MoveTest()
        {
            base.foreColor = ConsoleColor.Red;
            base.backColor = ConsoleColor.DarkRed;
        }
    }
}

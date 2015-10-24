using PuzzleConsole.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole.ActorTypes
{
    public class Grass:Actor
    {
        public Grass()
        {
            base.color = ConsoleColor.DarkGreen;
        }

        public override string CharacterRepresentation
        {
            get
            {
                return "░";
            }
        }
    }
}

using PuzzleConsole.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole.ActorTypes
{
    [DefaultCharacterRepresentation("X")]
    public class Custom : Actor
    {
        public Custom()
        {
            base.foreColor = ConsoleColor.White;
            base.backColor = ConsoleColor.Black;
        }

        private string customCharacter;

        public override string CharacterRepresentation
        {
            get
            {
                return customCharacter == null ? base.CharacterRepresentation : customCharacter;
            }
            set
            {
                customCharacter = value;
            }
        }

        public override void GameTick(EventArgs args)
        {

        }
    }
}

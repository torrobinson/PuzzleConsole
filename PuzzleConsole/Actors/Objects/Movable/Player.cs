﻿using PuzzleConsole.Game;
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
            base.foreColor = ConsoleColor.Red;
            base.backColor = ConsoleColor.Yellow;
        }

        public override string CharacterRepresentation
        {
            get
            {
               return "+";
            }
        }
    }
}

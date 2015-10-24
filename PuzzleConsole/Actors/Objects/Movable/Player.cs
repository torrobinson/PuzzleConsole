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
            base.characterRepresentation = "M";
            base.color = ConsoleColor.Red;
        }
    }
}

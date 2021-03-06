﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PuzzleConsole.Game;
using PuzzleConsole.Actions;
using PuzzleConsole.Actions.Actions;

namespace PuzzleConsole.ActorTypes
{
    public abstract class Follower : Pushable
    {

        public double moveActionsPerTick = 0.1; //1 block per sec
        public override double MoveActionsPerTick
        {
            get{
                return moveActionsPerTick;
            }
        }

        public Follower()
        {
            base.SubscribeToTicks();
        }
    }
}

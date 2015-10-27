using PuzzleConsole.ActorTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole.Actions
{
    public abstract class Action
    {
        public Command Command;

        public Action(Command command) {
            this.Command = command;
        }
        public Actor Actor() {
            return this.Command.Actor;
        }

        public abstract void Execute();
    }
}

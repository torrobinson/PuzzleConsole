using PuzzleConsole.ActorTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole.Actions.Actions
{
    public class MoveAction : Action
    {
        public Common.Direction Direction;

        public override double Speed
        {
            get{
                return Actor().MoveActionsPerTick;
            }
        }

        public MoveAction(Command command, Common.Direction direction) : base(command) {
            this.Direction = direction;
        }

        public override void Execute()
        {
            //Then the custom Move implementation
            if (Actor().IsMovable()) {
                ((Movable)Actor()).Move(Direction);
            }
        }
    }
}

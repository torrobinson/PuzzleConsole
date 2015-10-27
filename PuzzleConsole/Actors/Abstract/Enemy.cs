using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PuzzleConsole.Game;
using PuzzleConsole.Actions;
using PuzzleConsole.Actions.Actions;

namespace PuzzleConsole.ActorTypes
{
    public abstract class Enemy : Pushable
    {

        public double moveActionsPerTick = 0.05; //1 block per sec
        public override double MoveActionsPerTick
        {
            get{
                return moveActionsPerTick;
            }
        }

        public Enemy()
        {
            //Subscribe to the game clock
            base.SubscribeToTicks();

            //Throw in a command
            Command movementCommand = new Command();

            //Throw random movement into the command
            Random random = new Random();

            List<Common.Direction> possibleDirections = Common.GetAllDirections();

            List<PuzzleConsole.Actions.Action> movementActions = new List<PuzzleConsole.Actions.Action>();
            for (int i = 0; i < 10; i++) {
                movementActions.Add(
                    new MoveAction(movementCommand, 
                            possibleDirections[random.Next(0, possibleDirections.Count)]
                        )
                    );
            }

            movementCommand.AddActions(movementActions);

            this.AssignCommand(movementCommand);
        }
    }
}

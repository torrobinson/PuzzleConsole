using PuzzleConsole.Actions.Actions;
using PuzzleConsole.ActorTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole.Actions
{
    public class Command
    {
        public string Name;
        public Actor Actor;
        public Action LastAction;

        private List<Action> actionQueue;
        public List<Action> ActionQueue {
            get {
                return actionQueue;
            }
        }

        public Command() {
            LastAction = null;
            actionQueue = new List<Action>();
        }

        public bool HasActionToPerform() {
            return actionQueue.Any();
        }

        //Returns true if there was work to do still
        public bool ExecuteNextAction() {
            if (IsMovement() && IsTickTime(Actor.MoveActionsPerTick))
            {
                PuzzleConsole.Actions.Action nextAction = NextAction();
                if (nextAction != null)
                {
                    nextAction.Execute();
                    return true;
                }
            }

            return false;
        }

        private bool IsTickTime(double speed)
        {
            return Actor.Game.TickCount % (1 / speed) == 0;
        }

        // Action Queue manipulation
        public Type NextActionType() {
            if (actionQueue.Any())
                return actionQueue.First().GetType();
            else
                return null;
        }

        public bool IsMovement() {
            return NextActionType() == typeof(MoveAction);
        }

        public Action NextAction() {
            if (actionQueue.Any()) {
                Action action = actionQueue.First();
                actionQueue.Remove(action);
                LastAction = action;
                return action;
            }
            else
                return null;
        }
        public void AddAction(Action action) {
            actionQueue.Add(action);
        }

        public void AddActions(List<Action> actions)
        {
            actionQueue.AddRange(actions);
        }

        public void ClearActions() {
            actionQueue = new List<Action>();
        }
    }
}

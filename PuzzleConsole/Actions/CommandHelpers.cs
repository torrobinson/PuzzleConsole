using PuzzleConsole.Game;
using PuzzleConsole.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuzzleConsole.ActorTypes;
using PuzzleConsole.Actions.Actions;

namespace PuzzleConsole.Actions
{
    public static class CommandHelpers
    {
        public static Command CreateMoveToCommand(Actor actor,Point destination) {
            Command newCommand = new Command();

            byte[,] moveableAreas = PathFinderHelpers.createByteGridFromLayer(actor.Layer);

            PathFinder pathFinder = new PathFinder(moveableAreas);

            List<PathFinderNode> nodes = pathFinder.FindPath(
                    new System.Drawing.Point(actor.Location.X, actor.Location.Y),
                    new System.Drawing.Point(destination.X, destination.Y)
                );

            List<Common.Direction> directions = GetDirectionsFromNodeList(nodes);

            foreach(Common.Direction direction in directions){
                newCommand.AddAction(new MoveAction(newCommand, direction));
            }

            return newCommand;
        }

        private static List<Common.Direction> GetDirectionsFromNodeList(List<PathFinderNode> nodes) {
            List<Common.Direction> directions = new List<Common.Direction>();

            object lastNode = null;
            foreach (PathFinderNode node in nodes) {
                if (lastNode != null)
                {
                    directions.Add(Common.GetDirectionBetweenPoints(
                        new Point(((PathFinderNode)lastNode).X, ((PathFinderNode)lastNode).Y),
                        new Point(node.X,node.Y)
                    ));
                }
                lastNode= node;
            }

            return directions;
        }
    }
}

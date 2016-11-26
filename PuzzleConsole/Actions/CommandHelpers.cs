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
        public static Command CreateMoveToCommand(Actor actor,Point destination, bool tryTouchRegardless = false) {
            Command newCommand = new Command();

            //Create a 2D byte array from the layer the actor is on. 1s represent a movable tile
            byte[,] moveableAreas = PathFinderHelpers.createByteGridFromLayer(actor.Layer);

            //Initialize the pathfinder with the byte array
            PathFinder pathFinder = new PathFinder(moveableAreas);

            //Try find the nodes in the found path
            List<Common.Direction> directions;
            List<PathFinderNode> nodes = pathFinder.FindPath(
                   new System.Drawing.Point(actor.Location.X, actor.Location.Y),
                   new System.Drawing.Point(destination.X, destination.Y)
               );
            
            //If we didnt get a direct result but want to try the surrounding points
            if (nodes == null || !nodes.Any() && tryTouchRegardless)
            {
                //Since you can't pathfind ONTOP the actor, we will test each free block around the destination
                List<Point> placesToTry = new List<Point>() {
                    destination.Add(Common.DirectionToPointOffset(Common.Direction.Up)),
                    destination.Add(Common.DirectionToPointOffset(Common.Direction.Down)),
                    destination.Add(Common.DirectionToPointOffset(Common.Direction.Left)),
                    destination.Add(Common.DirectionToPointOffset(Common.Direction.Right))
                };

                List<PathFinderNode> quickestPath = null;
                int quickestPathCount = int.MaxValue;

                foreach (Point point in placesToTry)
                {
                    nodes = pathFinder.FindPath(
                        new System.Drawing.Point(actor.Location.X, actor.Location.Y),
                        new System.Drawing.Point(point.X, point.Y)
                    );

                    if ((nodes != null && nodes.Count() < quickestPathCount))
                    {
                        //Found a new quickest path
                        quickestPath = new List<PathFinderNode>();
                        quickestPath.AddRange(nodes);
                        quickestPathCount = nodes.Count();
                    }
                }
                nodes = quickestPath;
            }


            directions = GetDirectionsFromNodeList(nodes);
           

            //If there are actual moves to make
            if (directions.Any())
            {
                foreach (Common.Direction direction in directions)
                {
                    newCommand.AddAction(new MoveAction(newCommand, direction));
                }
            }
            else {
                return null;
            }
            
            return newCommand;
        }

        private static List<Common.Direction> GetDirectionsFromNodeList(List<PathFinderNode> nodes) {
            List<Common.Direction> directions = new List<Common.Direction>();

            if (nodes != null && nodes.Any())
            {
                object lastNode = null;
                foreach (PathFinderNode node in nodes)
                {
                    if (lastNode != null)
                    {
                        directions.Add(Common.GetDirectionBetweenPoints(
                            new Point(((PathFinderNode)lastNode).X, ((PathFinderNode)lastNode).Y),
                            new Point(node.X, node.Y)
                        ));
                    }
                    lastNode = node;
                }
            }
            return directions;
        }
    }
}

using PuzzleConsole.Game;
using PuzzleConsole.ActorTypes;
using PuzzleConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.Serialization;
using PuzzleConsole.Actions;
using PuzzleConsole.Actions.Actions;

namespace PuzzleConsole.ActorTypes
{
    [DefaultCharacterRepresentation(" ")]
    public abstract class Actor : IDisposable
    {
        //Positional
        public ActorLayer Layer;
        public Point Location;
        public Actor Represents; //if this actor is ever out of world but we still want it to represent a real actor in memory
        public Actor PushedActorLastMove;
        public List<Command> CommandQueue;
        public Command CurrentCommand;

        //@ 20 ticks per second: 0.1 bpt = 2 bps, 0.05 bpt = 1 bps,  0.5 bpt = 10 bps
        public abstract double MoveActionsPerTick
        {
            get;
        }

        //Attributes
        public bool Static = true;
        public bool Visible = true;
        public bool Clippable = false; //can be walked through
        //public int Zindex = 0; //not implemented yet

        public virtual string CharacterRepresentation
        {
            get { return ((DefaultCharacterRepresentation)Attribute.GetCustomAttribute(this.GetType(), typeof(DefaultCharacterRepresentation))).character; }
            set {}
        }

        public GameInstance Game
        {
            get{
                return this.Layer.GameInstance;
            }
        }

        public ConsoleColor foreColor = ConsoleColor.White;
        public ConsoleColor backColor = ConsoleColor.Black;

        public Actor() {
            CommandQueue = new List<Command>();
        }

        public void AssignCommand(Command command) {
            command.Actor = this;
            CommandQueue.Add(command);
        }

        public void ClearCommands() {
            CommandQueue = new List<Command>();
        }

        public Command GetNextCommand() {
            Command command = CommandQueue.First();
            CommandQueue.Remove(command);
            CurrentCommand = command;
            return command;
        }

        public void SubscribeToTicks() {
            GameInstance.TickHandler += (sender, args) => GameTick(args);
        }

        public void UnsubscribeFromTicks() {
            GameInstance.TickHandler -= (sender, args) => GameTick(args);
        }

        public void GameTick(EventArgs args)
        {
            //If we're out of commands then attempt to fetch the next one
            if (CurrentCommand == null)
            {
                if (CommandQueue.Any())
                {
                    //Pop the next command off the stack
                    CurrentCommand = GetNextCommand();
                }
                else
                {
                    CurrentCommand = null;
                }
            }


            //Try execute any work left in the command
            if (CurrentCommand != null && CurrentCommand.HasActionToPerform())
            {
                CurrentCommand.ExecuteNextAction();
            }
        }

        //Get the location of this object, plus an offset
		public Point GetLocationAtOffset(int offsetx, int offsety){
            return Location.Add(new Point(offsetx, offsety));
		}

        //Get current location as a Point
		public Point GetLocation(){
			return new Point (
				Location.X,
				Location.Y
			);	
		}

        //Return whether there is anything below this actor
        public Actor GetFirstObjectBelow(){
            if (Layer == null) {
                return null;
            }

            IEnumerable<ActorLayer> possibleBackgroundLayers = Game.Layers.Where(l => l.ZIndex < this.Layer.ZIndex);
            ActorLayer layerBelow = null;
            
            if(possibleBackgroundLayers.Any()){
                layerBelow = possibleBackgroundLayers.First();
            }

            if(layerBelow == null){
                //If no layer was below
                return null;
            }
            else{
                Actor actorBelow = layerBelow.GetObjectAtPoint(this.Location);
                if(actorBelow == null){
                    //If nothing was below
                    return null;
                }
                else{
                    return actorBelow;
                }
            }
        }

        //Returns the object (if any) 1 tile away from this object in the given direction
		public Actor GetObjectInDirection(PuzzleConsole.Common.Direction direction){
			Point offset = PuzzleConsole.Common.DirectionToPointOffset(direction);
            return Layer.GetObjectAtPoint(GetLocationAtOffset(offset.X,offset.Y));
		}

        //Override the ToString for debugging and console rendering
        public override string ToString() {
            return CharacterRepresentation;
        }

        public void Dispose() {
            UnsubscribeFromTicks();
        }

        public bool IsPushable() {
            return this.GetType().IsSubclassOf(typeof(Pushable));
        }

        public bool IsMovable(){
            return this.GetType().IsSubclassOf(typeof(Movable));
        }
    }

    public static class ActorHelpers {

        public static Type GetSubclassForStringRepresentation(string representation)
        {
            foreach (Type typ in Assembly.GetAssembly(typeof(Actor)).GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Actor))))
            {
                if (((DefaultCharacterRepresentation)Attribute.GetCustomAttribute(typ, typeof(DefaultCharacterRepresentation))).character == representation)
                {
                    {
                        return typ;
                    }
                }
            }
            return null;
        }

    }
}

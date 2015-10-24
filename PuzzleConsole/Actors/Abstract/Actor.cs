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

namespace PuzzleConsole.ActorTypes
{
    public abstract class Actor : IDisposable
    {
        //Positional
        public ActorLayer Layer;
        public Point Location;
        public Actor Represents; //if this actor is ever out of world but we still want it to represent a real actor in memory

        //Attributes
        public bool Static = true;
        public bool Visible = true;
        public bool Clippable = false; //can be walked through
        //public int Zindex = 0; //not implemented yet

        public virtual string CharacterRepresentation
        {
            get {return " ";}
            set {}
        }

        public ConsoleColor foreColor = ConsoleColor.White;
        public ConsoleColor backColor = ConsoleColor.Black;

        public Actor() {
        }

        public void SubscribeToTicks() {
            GameInstance.TickHandler += (sender, args) => GameTick(args);
        }

        public void UnsubscribeFromTicks() {
            GameInstance.TickHandler -= (sender, args) => GameTick(args);
        }

        public void GameTick(EventArgs args){
            
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

        //Return whether there is anything above this actor, given other layers to check
        public Actor GetFirstObjectBelow(){
            //Find the first layer below

            if (Layer == null) {
                return null;
            }

            IEnumerable<ActorLayer> possibleBackgroundLayers = Layer.GameInstance.Layers.Where(l => l.ZIndex < this.Layer.ZIndex);
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

        //Returns the object (if any) 1 tile away from this object int he given direction
		public Actor GetObjectInDirection(PuzzleConsole.Common.Direction direction){
			Point offset = PuzzleConsole.Common.DirectionToPointOffset(direction);
            return Layer.GetObjectAtPoint(GetLocationAtOffset(offset.X,offset.Y));
		}

        //Refreshes the parent World with the updated location of this item
        public void UpdateWorldObjectsArrayWithNewLocation(Point oldLocation){
            //remove from old location
            Layer.Actors[oldLocation.Y][oldLocation.X] = new Empty();

            //insert into new location
            Layer.Actors[Location.Y][Location.X] = this;
        }

        //Override the ToString for debugging and console rendering
        public override string ToString() {
            return CharacterRepresentation;
        }

        public void Dispose() {
            UnsubscribeFromTicks();
        }
    }

    public static class WorldObjectHelpers {
   
        public static Type GetSubclassForStringRepresentation(string representation) {
            foreach(Type typ in Assembly.GetAssembly(typeof(Actor)).GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Actor)))){
                if (((Actor)Activator.CreateInstance(typ)).CharacterRepresentation == representation) {
                    return typ;
                }
            }
            return null;
        }

    }
}

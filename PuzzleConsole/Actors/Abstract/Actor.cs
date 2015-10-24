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
    public abstract class Actor
    {
        //Positional
        public ActorLayer Layer;
        public Point Location;

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

        public ConsoleColor color = ConsoleColor.White;

        public Actor() {
           
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
        public bool IsAnythingAbove(List<ActorLayer> layers){
            //Go through any layers with a higher zindex
            foreach(ActorLayer layer in layers.Where(l => l.ZIndex > this.Layer.ZIndex)){
                //And check to see if another object occupies this same location
                if(!layer.IsSpaceFree(this.Location.X, this.Location.Y)){
                    return true;
                }
            }
                return false;
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

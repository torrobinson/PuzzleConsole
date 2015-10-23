using PuzzleConsole.Game;
using PuzzleConsole.WorldTypes;
using PuzzleConsole;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace PuzzleConsole.WorldTypes
{

    public abstract class WorldObject
    {
        //Positional
        public World World;
        public Point Location;

        //Attributes
        public bool Static = true;
        public bool Clippable= false;
        public bool Visible = true;
        //public int Zindex = 0; //not implemented yet

        public string characterRepresentation = " ";

        public WorldObject() {
           
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

        //Returns the object (if any) 1 tile away from this object int he given direction
		public WorldObject GetObjectInDirection(PuzzleConsole.Common.Direction direction){
			Point offset = PuzzleConsole.Common.DirectionToPointOffset(direction);
            return World.GetObjectAtPoint(GetLocationAtOffset(offset.X,offset.Y));
		}

        //Refreshes the parent World with the updated location of this item
        public void UpdateWorldObjectsArrayWithNewLocation(Point oldLocation){
            //remove from old location
            World.Objects[oldLocation.Y][oldLocation.X] = null;

            //insert into new location
            World.Objects[Location.Y][Location.X] = this;
        }

        //Override the ToString for debugging and console rendering
        public override string ToString() {
            return characterRepresentation;
        }
    }

    public static class WorldObjectHelpers {
   
        public static Type GetSubclassForStringRepresentation(string representation) {
            foreach(Type typ in Assembly.GetAssembly(typeof(WorldObject)).GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(WorldObject)))){
                if (((WorldObject)Activator.CreateInstance(typ)).characterRepresentation == representation) {
                    return typ;
                }
            }
            return null;
        }

    }
}

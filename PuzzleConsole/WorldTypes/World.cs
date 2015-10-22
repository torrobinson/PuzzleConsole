using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuzzleConsole.Game;
using System.IO;

namespace PuzzleConsole.WorldTypes
{
    public class World
    {
        public List<List<WorldObject>> Objects = new List<List<WorldObject>>();
        public int width = 0;
        public int height = 0;

        public World(int width, int height) {
            this.width = width;
            this.height = height;
        }

        private void InitializeAsEmpty(int width, int height) {
            Objects = new List<List<WorldObject>>();
            for (var h = 0; h < height; h++)
            {
                List<WorldObject> row = new List<WorldObject>();
                for (var w = 0; w < width; w++)
                {
                    row.Add(null);
                }
                Objects.Add(row);
            }
            this.width = width;
            this.height = height;
        }
        public void InitializeFromFile(string filename) {
            //Perform load
            string[] maplines = File.ReadAllLines(filename);

            //Figure out height and width
            int height = maplines.Length;
            int width = 0;
            foreach (string line in maplines) { //find the longest row
                if (line.Count() > width)
                    width = line.Count();
            }

            //Empty the world at the maximum size needed
            InitializeAsEmpty(width, height);

            //Fill with Objects from the map file
            int x = 0;
            int y = 0;
            foreach (string line in maplines)
            {
                foreach (char character in line)
                {
                    string cell = character.ToString();
                    
                    //Determine the object to add to the map
                    switch (cell) {
                        case "#":
                            AddObject(new Wall(), x, y);
                            break;

                        case "+":
                            AddObject(new Player(), x, y);
                            break;

                        case "@":
                            AddObject(new Stone(), x, y);
                            break;
                    }

                    x++;
                }
                x = 0;
                y++;
            }
        }

        public void AddObject(WorldObject obj, int x, int y) {
            if (IsSpaceFree(x,y))
            {
                Point here = new Point(x,y);

				//Inject into world
				SetObjectAtPoint(obj, here);

				//Set the object's world to this world
				obj.World = this;
                obj.Location = here;
            }
        }

        public bool IsSpaceFree(int x, int y) {

            if (x < 0 || y < 0) return false;
            if (x > Objects[0].Count() || y > Objects.Count()) return false;

            return Objects == null || Objects[y][x] == null;
        }



		public WorldObject GetObjectAtPoint(int x, int y){
			return Objects [y][x];
		}
		public WorldObject GetObjectAtPoint(Point point){
			return GetObjectAtPoint(point.X, point.Y);
		}

		public void SetObjectAtPoint(WorldObject obj, int x, int y){
			Objects[y][x] = obj;
		}
		public void SetObjectAtPoint(WorldObject obj, Point point){
			SetObjectAtPoint(obj,point.X, point.Y);
		}

        public WorldObject GetObjectAt(int x, int y) {
            return Objects[y][x];
        }

        public WorldObject FindFirstObjectInWorldOfType(Type theType) {
            foreach (List<WorldObject> row in Objects) {
                foreach(WorldObject obj in row.Where(o=>o!=null)){
                    if (theType == obj.GetType() || obj.GetType().IsSubclassOf(theType))
                        return obj;
                }
            }
            return null;
        }

    }
}

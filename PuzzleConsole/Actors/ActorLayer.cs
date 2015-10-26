using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuzzleConsole.Game;
using System.IO;

namespace PuzzleConsole.ActorTypes
{
    [Serializable()]
    public class ActorLayer
    {
        public List<List<Actor>> Actors = new List<List<Actor>>();
        public int Width = 0;
        public int Height = 0;
        public int ZIndex = 0; // -1 = below base, 0 = base, 1 = above base
        public bool AlwaysCentered; //if true, this layer will always appear centered on screen
        public bool Visible = true;
        public string Name = "Layer";
        public GameInstance GameInstance;

        public ActorLayer(string name, int zIndex, GameInstance game) {
            this.ZIndex = zIndex;
            this.Name = name;
            this.GameInstance = game;
        }

        private void InitializeAsEmpty(int width, int height) {
            Actors = new List<List<Actor>>();
            for (var h = 0; h < height; h++)
            {
                List<Actor> row = new List<Actor>();
                for (var w = 0; w < width; w++)
                {
                    row.Add(new Empty());
                }
                Actors.Add(row);
            }
            this.Width = width;
            this.Height = height;
        }
        public void InitializeFromFile(string filename) {
            //Perform load
            string[] maplines = File.ReadAllLines(filename);

            //Figure out height and width
            int height = maplines.Length;
            int width = maplines.Select(l => l.Length).Max();

            //Empty the world at the maximum size needed
            InitializeAsEmpty(width, height);

            //Fill with Objects from the map file
            int x = 0;
            int y = 0;
            foreach (string line in maplines)
            {
                foreach (char character in line)
                {
                    //Find the type for this character
                    Type typeToInsert = ActorHelpers.GetSubclassForStringRepresentation(character.ToString());
                    if (typeToInsert != null)
                    {
                        //If we found one, then inject a new one into the world
                        Actor objToInsert = (Actor)Activator.CreateInstance(typeToInsert);
                        AddObject(objToInsert, x, y);
                    }
                    else {
                        //Type not recognized, so just add a wall/static item with the same character
                        Custom newCustom = new Custom();
                        newCustom.CharacterRepresentation = character.ToString();
                        AddObject(newCustom, x, y);
                    }
                    x++;
                }
                x = 0;
                y++;
            }
        }

        public void AddObject(Actor obj, int x, int y) {
            if (IsSpaceFree(x,y))
            {
                Point specifiedPoint = new Point(x,y);

				//Inject into world
                SetObjectAtPoint(obj, specifiedPoint);

				//Set the object's world to this world
				obj.Layer = this;
                obj.Location = specifiedPoint;
            }
        }

        public bool IsSpaceFree(int x, int y) {

            if (Actors[0] == null) return false; //if theres nost even a row here
            if (x < 0 || y < 0) return false;    //if the space is out of lower bounds
            if (y > Actors.Count() - 1 || x > Actors[y].Count() - 1) return false; //if the space is out of higher bounds

            //Space is available to check

            return Actors == null || Actors[y] == null || Actors[y][x].GetType() == typeof(Empty); 
        }



		public Actor GetObjectAtPoint(int x, int y){
            if (x < 0 || y < 0) return null;
            if (y >= Actors.Count || x >= Actors[y].Count) return null; 
            return Actors[y][x];
		}
		public Actor GetObjectAtPoint(Point point){
			return GetObjectAtPoint(point.X, point.Y);
		}

		public void SetObjectAtPoint(Actor obj, int x, int y){
			Actors[y][x] = obj;
		}
		public void SetObjectAtPoint(Actor obj, Point point){
			SetObjectAtPoint(obj,point.X, point.Y);
		}

        public Actor GetObjectAt(int x, int y) {
            return Actors[y][x];
        }

        public Actor FindFirstObjectInWorldOfType(Type theType) {
            foreach (List<Actor> row in Actors) {
                foreach(Actor obj in row.Where(o=>o!=null)){
                    if (theType == obj.GetType() || obj.GetType().IsSubclassOf(theType))
                        return obj;
                }
            }
            return null;
        }

        public ActorLayer Clone() {
            ActorLayer copyLayer = new ActorLayer(this.Name, this.ZIndex, this.GameInstance);
            copyLayer.Height = this.Height;
            copyLayer.Width = this.Width;
            copyLayer.AlwaysCentered = this.AlwaysCentered;

            foreach (List<Actor> row in this.Actors)
            {
                List<Actor> newRow = new List<Actor>(row);
                copyLayer.Actors.Add(newRow);
            }
            return copyLayer;
        }

        public string Value()
        {
            string grid = "";

            foreach (List<Actor> row in Actors)
            {
                grid += String.Join("", row) + System.Environment.NewLine;
            }

            return grid;
        }

    }
}

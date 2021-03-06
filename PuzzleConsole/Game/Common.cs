﻿using System;
using PuzzleConsole.Game;
using PuzzleConsole.ActorTypes;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PuzzleConsole
{
	public static class Common
	{
		public enum Direction
		{
			Up,
			Down,
			Left,
			Right,
            UpLeft,
            UpRight,
            DownLeft,
            DownRight
		};

        public static Direction GetDirectionBetweenPoints(Point one, Point two) {
            Point offset = two.Subtract(one);
            if(offset.X == 0 && offset.Y == -1){
                return Direction.Up;
            }

            else if (offset.X == 0 && offset.Y == 1)
            {
                return Direction.Down;
            }

            else if (offset.X == -1 && offset.Y == 0)
            {
                return Direction.Left;
            }

            else if (offset.X == 1 && offset.Y == 0)
            {
                return Direction.Right;
            }

            else if (offset.X == -1 && offset.Y == -1)
            {
                return Direction.UpLeft;
            }

            else if (offset.X == 1 && offset.Y == -1)
            {
                return Direction.UpRight;
            }

            else if (offset.X == -1 && offset.Y == 1)
            {
                return Direction.DownLeft;
            }

            else if (offset.X == 1 && offset.Y == 1)
            {
                return Direction.DownRight;
            }

            else {
                return Direction.Up;
            }
        }

        public static List<Common.Direction> GetAllDirections(){
            return new List<Common.Direction>
            {
                Common.Direction.Up,
                Common.Direction.Right,
                Common.Direction.Down,
                Common.Direction.Left,
                Common.Direction.UpLeft,
                Common.Direction.UpRight,
                Common.Direction.DownLeft,
                Common.Direction.DownRight
            };
        }

        public static List<Common.Direction> GetCardinalDirections()
        {
            return new List<Common.Direction>
            {
                Common.Direction.Up,
                Common.Direction.Right,
                Common.Direction.Down,
                Common.Direction.Left,
            };
        }

        public static List<Common.Direction> GetDiagonalDirections()
        {
            return new List<Common.Direction>
            {
                Common.Direction.UpLeft,
                Common.Direction.UpRight,
                Common.Direction.DownLeft,
                Common.Direction.DownRight
            };
        } 

        public static List<List<Actor>> CreateLayer(int height, int width, Type defaultType = null)
        {
            List<List<Actor>> layer = new List<List<Actor>>();

            for(int y = 0; y < height; y++){
                List<Actor> row = new List<Actor>();

                for(int x = 0; x < width; x++){
                    row.Add(
                        defaultType == null ? null : (Actor)Activator.CreateInstance(defaultType)
                        );
                }

                layer.Add(row);
            }

            return layer;
        }

		public static Point DirectionToPointOffset(Direction direction){
			int offsetx = 0;
			int offsety = 0;

			switch (direction) {

			case PuzzleConsole.Common.Direction.Up:
				offsetx = 0;
				offsety = -1;	
				break;

			case PuzzleConsole.Common.Direction.Down:
				offsetx = 0;
				offsety = 1;	
				break;

			case PuzzleConsole.Common.Direction.Left:
				offsetx = -1;
				offsety = 0;	
				break;

			case PuzzleConsole.Common.Direction.Right:
				offsetx = 1;
				offsety = 0;	
				break;

            case PuzzleConsole.Common.Direction.UpLeft:
                offsetx = -1;
                offsety = -1;
                break;

            case PuzzleConsole.Common.Direction.UpRight:
                offsetx = 1;
                offsety = -1;
                break;

            case PuzzleConsole.Common.Direction.DownLeft:
                offsetx = -1;
                offsety = 1;
                break;

            case PuzzleConsole.Common.Direction.DownRight:
                offsetx = 1;
                offsety = 1;
                break;

			default:
				break;
			}

			return new Point (
				offsetx,
				offsety
			);

		}

        public static ActorLayer CutLayerToSize(ActorLayer layer, Point center, int width, int height)
        {
           //Copy the original to a new layer
           ActorLayer original = layer.Clone();

           layer.Width = width;
           layer.Height = height;

           //Set the layer to the new cut size
           layer.Actors = CreateLayer(height, width);

            //Start going through and writing the trimmed cells from the 
            //  original to the trimmed layer passed in
            int trimmedXToWrite = 0;
            int trimmedYToWrite = 0;

            int topPosition = center.Y -Convert.ToInt32(Math.Floor((decimal)height/2));
            int bottomPosition = center.Y + Convert.ToInt32(Math.Ceiling((decimal)height / 2));
            int leftPosition = center.X - Convert.ToInt32(Math.Floor((decimal)width / 2));
            int rightPosition = center.X + Convert.ToInt32(Math.Ceiling((decimal)width / 2));


            for(int y = topPosition;  y < bottomPosition; y++){
                    for (int x = leftPosition; x < rightPosition; x++){
                        if (x >= 0 && x < original.Width && y >= 0 && y < original.Height)
                        {
                            layer.Actors[trimmedYToWrite][trimmedXToWrite] = original.Actors[y][x];
                        }
                        else
                        {
                            layer.Actors[trimmedYToWrite][trimmedXToWrite] = new OutOfBounds();
                        }
                        
                        trimmedXToWrite++;

                        }

                    trimmedXToWrite = 0;
                    trimmedYToWrite++;
            }
            return layer;
        }
	}
}


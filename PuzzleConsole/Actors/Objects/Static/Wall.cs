using PuzzleConsole.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleConsole.ActorTypes
{
    public class Wall : Actor
    {
        private string customCharacter;

        private string udlr = "╬";
        private string ur = "╚";
        private string ul = "╝";
        private string dl = "╗";
        private string dr = "╔";
        private string lr = "═";
        private string ud = "║";
        private string udl = "╣";
        private string udr = "╠";
        private string ulr = "╩";
        private string dlr = "╦";

        public Wall() {
            base.color = ConsoleColor.DarkGray;
        }

        public override string CharacterRepresentation
        {
            get
            {
                //If not part of a world, use the generic wall block
                if (this.Layer == null)
                {
                    return "█";
                }
                else {
                    if (String.IsNullOrEmpty(customCharacter))
                    {
                        //this wall is within a world. Base it's string on the walls around its
                        bool u = HasWallUp();
                        bool d = HasWallDown();
                        bool l = HasWallLeft();
                        bool r = HasWallRight();


                        //Combos
                        if (u) {
                            if (d && l && r) return udlr;
                            if (d && l) return udl;
                            if (d && r) return udr;
                            if (l && r) return ulr;
                            if (r) return ur;
                            if (l) return ul;
                            if (d) return ud;
                        }
                        if (d) {
                            if (l && r) return dlr;
                            if (l) return dl;
                            if (r) return dr;
                        }
                        if (l && r) return lr;

                        //Check if the wall should at least be a vertical or horizonal line based on the first closest piece found
                        if (l || r) return lr;
                        if (u || d) return ud;
                        return ud;
                    }
                    else {
                        return customCharacter;
                    }
                }
            }

            set {
                customCharacter = value;
            }
        }

        private bool HasWall(Common.Direction direction){
            Actor possibleWall = GetObjectInDirection(direction);
            return possibleWall != null && possibleWall.GetType() == typeof(Wall);
        }
        private bool HasWallUp() {
            return HasWall(Common.Direction.Up);
        }

        private bool HasWallDown()
        {
            return HasWall(Common.Direction.Down);
        }

        private bool HasWallLeft()
        {
            return HasWall(Common.Direction.Left);
        }

        private bool HasWallRight()
        {
            return HasWall(Common.Direction.Right);
        }
    }
}

using System;
using PuzzleConsole.Game;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PuzzleConsole.ActorTypes;

namespace PuzzleConsole
{
    class Program
    {

        static void Main(string[] args)
        {
            //Create a new game
            GameInstance newGame = new GameInstance();

            //Start the game
            newGame.Initialize();
        }
    }
}

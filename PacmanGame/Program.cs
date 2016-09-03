using System;
using PacmanGame.Model;
using PacmanGame.UserInterfaceLayer;

namespace PacmanGame
{
    class Program
    {
        static void Main(string[] args)
        {
            bool isCurrentGameOver = false;
            GameFieldUserInterface gf = new GameFieldUserInterface(1, 500, 1, 1);
            ConsoleKeyInfo consoleKey = new ConsoleKeyInfo();
            do
            {
                StepOperationResult result;
                while (Console.KeyAvailable == false)
                {
                    if (consoleKey.Key == ConsoleKey.LeftArrow || consoleKey.Key == ConsoleKey.RightArrow ||
                        consoleKey.Key == ConsoleKey.UpArrow || consoleKey.Key == ConsoleKey.DownArrow)
                    {
                        gf.MakeStep(out result, consoleKey.Key);
                    }
                    else
                    {
                        gf.MakeStep(out result);
                    }
                    if (result == StepOperationResult.GameOver 
                        || result == StepOperationResult.PacmanWins)
                    {
                        isCurrentGameOver = true;
                        break;
                    }
                }
                if (!isCurrentGameOver)
                {
                    consoleKey = Console.ReadKey(true);
                    gf.MakeStep(out result, consoleKey.Key);
                }
            } while (consoleKey.Key != ConsoleKey.Q && !isCurrentGameOver);

        }
    }
}

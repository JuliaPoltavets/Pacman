using System;
using PacmanGame.Model;
using PacmanGame.UserInterfaceLayer;

namespace PacmanGame
{
    class Program
    {
        static void Main(string[] args)
        {
            new GameFieldUserInterface(1, 500, 2, 1).Run();
        }
    }
}

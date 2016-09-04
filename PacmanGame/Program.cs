using System;
using PacmanGame.Model;
using PacmanGame.UserInterfaceLayer;

namespace PacmanGame
{
    class Program
    {
        static void Main(string[] args)
        {
            GameFieldUserInterface.GetInitialSettingsFromUser();
            GameFieldUserInterface.StartGame();
        }
    }
}

using System;
using PacmanGame.Model;

namespace PacmanGame
{
    class Program
    {
        static void Main(string[] args)
        {

            GameField gf = new GameField();
            gf.InitLevel(1, 1, 1);
            Console.SetWindowSize(gf._currentLevel._levelWidth+1,gf._currentLevel._levelHeight);
            for (int i = 0; i < gf._currentLevel._levelHeight; i++)
            {
                for (int j = 0; j < gf._currentLevel._levelWidth; j++)
                {
                    if (gf._currentLevel._level[i, j]._characterId == UniqueTypeIdentifiers.Dot)
                    {
                        Console.Write((char)183);
                    }
                    if (gf._currentLevel._level[i, j]._characterId == UniqueTypeIdentifiers.Pacman)
                    {
                        Console.Write("@");
                    }
                    if (gf._currentLevel._level[i, j]._characterId == UniqueTypeIdentifiers.Obstacle)
                    {
                        Console.Write((char)166);
                    }
                    if (gf._currentLevel._level[i, j]._characterId == UniqueTypeIdentifiers.Ghost)
                    {
                        Console.Write("8");
                    }
                }
                Console.WriteLine();
            }
            gf.MoveCharacter(MoveDirections.Left, UniqueTypeIdentifiers.Pacman, 0);
            for (int i = 0; i < gf._currentLevel._levelHeight; i++)
            {
                for (int j = 0; j < gf._currentLevel._levelWidth; j++)
                {
                    var currentCellCharId = gf._currentLevel._level[i, j]._characterId;
                    if (currentCellCharId  == UniqueTypeIdentifiers.Dot)
                    {
                        Console.Write((char)183);
                    }
                    if (currentCellCharId == UniqueTypeIdentifiers.EmptyCell || currentCellCharId == UniqueTypeIdentifiers.None)
                    {
                        Console.Write(' ');
                    }
                    if ((currentCellCharId & UniqueTypeIdentifiers.Pacman) == UniqueTypeIdentifiers.Pacman)
                    {
                        Console.Write("@");
                    }
                    if (currentCellCharId == UniqueTypeIdentifiers.Obstacle)
                    {
                        Console.Write((char)166);
                    }
                    if ((currentCellCharId & UniqueTypeIdentifiers.Ghost) == UniqueTypeIdentifiers.Ghost)
                    {
                        Console.Write("8");
                    }

                }
                Console.WriteLine();
            }
            gf.MoveCharacter(MoveDirections.Right, UniqueTypeIdentifiers.Pacman, 0);
            for (int i = 0; i < gf._currentLevel._levelHeight; i++)
            {
                for (int j = 0; j < gf._currentLevel._levelWidth; j++)
                {
                    var currentCellCharId = gf._currentLevel._level[i, j]._characterId;
                    if (currentCellCharId == UniqueTypeIdentifiers.Dot)
                    {
                        Console.Write((char)183);
                    }
                    if (currentCellCharId == UniqueTypeIdentifiers.EmptyCell || currentCellCharId == UniqueTypeIdentifiers.None)
                    {
                        Console.Write(' ');
                    }
                    if ((currentCellCharId & UniqueTypeIdentifiers.Pacman) == UniqueTypeIdentifiers.Pacman)
                    {
                        Console.Write("@");
                    }
                    if (currentCellCharId == UniqueTypeIdentifiers.Obstacle)
                    {
                        Console.Write((char)166);
                    }
                    if ((currentCellCharId & UniqueTypeIdentifiers.Ghost) == UniqueTypeIdentifiers.Ghost)
                    {
                        Console.Write("8");
                    }
                }
                Console.WriteLine();
            }
            gf.MoveCharacter(MoveDirections.Left, UniqueTypeIdentifiers.Pacman, 0);
            for (int i = 0; i < gf._currentLevel._levelHeight; i++)
            {
                for (int j = 0; j < gf._currentLevel._levelWidth; j++)
                {
                    var currentCellCharId = gf._currentLevel._level[i, j]._characterId;
                    if (currentCellCharId == UniqueTypeIdentifiers.Dot)
                    {
                        Console.Write((char)183);
                    }
                    if (currentCellCharId == UniqueTypeIdentifiers.EmptyCell || currentCellCharId == UniqueTypeIdentifiers.None)
                    {
                        Console.Write(' ');
                    }
                    if ((currentCellCharId & UniqueTypeIdentifiers.Pacman) == UniqueTypeIdentifiers.Pacman)
                    {
                        Console.Write("@");
                    }
                    if (currentCellCharId == UniqueTypeIdentifiers.Obstacle)
                    {
                        Console.Write((char)166);
                    }
                    if ((currentCellCharId & UniqueTypeIdentifiers.Ghost) == UniqueTypeIdentifiers.Ghost)
                    {
                        Console.Write("8");
                    }
                }
                Console.WriteLine();
            }


        }
    }
}

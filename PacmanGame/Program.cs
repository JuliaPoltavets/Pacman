﻿using System;
using PacmanGame.Model;

namespace PacmanGame
{
    class Program
    {
        static void Main(string[] args)
        {

            GameField gf = new GameField();
            gf.InitLevel(0, 1, 2);
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
                //gf.MovePacman(MoveDirections.Left);
                Console.WriteLine();
            }

        }
    }
}

using System;
using PacmanGame.Model;

namespace PacmanGame.UserInterfaceLayer
{
    public class GameFieldUserInterface
    {
        private GameField _gameField;
        private int _speed;

        public GameFieldUserInterface(short levelId, int speed, int ghostCount, int playerCount)
        {
            _gameField = new GameField();
            _gameField.InitLevel(levelId, playerCount, ghostCount);
            _speed = speed;
        }

        public int Run()
        {
            Console.SetWindowSize(_gameField._currentLevel._levelWidth + 1, _gameField._currentLevel._levelHeight);
            for (int i = 0; i < _gameField._currentLevel._levelHeight; i++)
            {
                GameFieldUserInterface.PrintFieldLine(_gameField._currentLevel, i);
            }
            System.Threading.Thread.Sleep(_speed);
            _gameField.MoveCharacter(MoveDirections.Left, UniqueTypeIdentifiers.Pacman, 0);
            System.Threading.Thread.Sleep(_speed);
            for (int i = 0; i < _gameField._currentLevel._levelHeight; i++)
            {
                GameFieldUserInterface.PrintFieldLine(_gameField._currentLevel, i);
            }
            _gameField.MoveCharacter(MoveDirections.Right, UniqueTypeIdentifiers.Pacman, 0);
            System.Threading.Thread.Sleep(_speed);
            for (int i = 0; i < _gameField._currentLevel._levelHeight; i++)
            {
                GameFieldUserInterface.PrintFieldLine(_gameField._currentLevel, i);
            }
            _gameField.MoveCharacter(MoveDirections.Left, UniqueTypeIdentifiers.Pacman, 0);
            System.Threading.Thread.Sleep(_speed);
            for (int i = 0; i < _gameField._currentLevel._levelHeight; i++)
            {
                GameFieldUserInterface.PrintFieldLine(_gameField._currentLevel, i);
            }
            return 0;
        }


        public static void PrintGameField(Level gameField)
        {
            Console.Clear();
            for (int i = 0; i < gameField._levelHeight; i++)
            {
                for (int j = 0; j < gameField._levelWidth; j++)
                {
                    var currentCellCharId = gameField._level[i, j]._characterId;
                    Console.Write(MapCharTypeToCharUi(currentCellCharId));
                }
                Console.WriteLine();
            }
        }

        public static void PrintFieldLine(Level gameField, int lineIndex)
        {
            for (int j = 0; j < gameField._levelWidth; j++)
            {
                Console.SetCursorPosition(j, lineIndex);
                var currentCellCharId = gameField._level[lineIndex,j]._characterId;
                Console.Write(MapCharTypeToCharUi(currentCellCharId));
            }
        }

        public static void PrintSetOfCells(Level gameField, Position[] setOfCells)
        {
            foreach (Position coord in setOfCells)
            {
                Console.SetCursorPosition(coord._y,coord._x);
                var currentCellCharId = gameField._level[coord._y, coord._x]._characterId;
                Console.Write(MapCharTypeToCharUi(currentCellCharId));
            }
        }

        private static char MapCharTypeToCharUi(UniqueTypeIdentifiers type)
        {
            char charCode = ' ';
            if (type == UniqueTypeIdentifiers.Dot)
            {
                charCode = (char) CharactersUserInterface.Dot;
            }
            if (type == UniqueTypeIdentifiers.EmptyCell || type == UniqueTypeIdentifiers.None)
            {
                charCode = (char)CharactersUserInterface.EmptyCell;
            }
            if ((type & UniqueTypeIdentifiers.Pacman) == UniqueTypeIdentifiers.Pacman)
            {
                charCode = (char)CharactersUserInterface.Pacman;
            }
            if (type == UniqueTypeIdentifiers.Obstacle)
            {
                charCode = (char)CharactersUserInterface.Obstacle;
            }
            if ((type & UniqueTypeIdentifiers.Ghost) == UniqueTypeIdentifiers.Ghost)
            {
                charCode = (char)CharactersUserInterface.Ghost;
            }
            return charCode;
        }
    }
}
using System;
using PacmanGame.Model;

namespace PacmanGame.UserInterfaceLayer
{
    public class GameFieldUserInterface
    {
        private GameField _gameField;
        private readonly int _speed;
        private readonly int _ghostCount;
        private MoveDirections _defaultGhostDirection = MoveDirections.Up;

        public GameFieldUserInterface(short levelId, int speed, int ghostCount, int playerCount)
        {
            _speed = speed;
            _ghostCount = ghostCount;
            _gameField = new GameField();
            _gameField.InitLevel(levelId, playerCount, ghostCount);
            PrintGameField(_gameField._currentLevel);
        }

        public void MakeStep(out StepOperationResult result, ConsoleKey key = ConsoleKey.LeftArrow, int playerId = 0)
        {
            result = StepOperationResult.None;

            if (key == ConsoleKey.LeftArrow)
            {
                result = _gameField.MoveCharacter(MoveDirections.Left, UniqueTypeIdentifiers.Pacman, playerId);
            }
            if (key == ConsoleKey.RightArrow)
            {
                result = _gameField.MoveCharacter(MoveDirections.Right, UniqueTypeIdentifiers.Pacman, playerId);
            }
            if (key == ConsoleKey.DownArrow)
            {
                result = _gameField.MoveCharacter(MoveDirections.Down, UniqueTypeIdentifiers.Pacman, playerId);
            }
            if (key == ConsoleKey.UpArrow)
            {
                result = _gameField.MoveCharacter(MoveDirections.Up, UniqueTypeIdentifiers.Pacman, playerId);
            }
            if (result == StepOperationResult.GameOver || result == StepOperationResult.PacmanWins)
            {
                return;
            }
            if (result == StepOperationResult.PacmanDied)
            {
                PrintGameField(_gameField._currentLevel);

                return;
            }

            for (int ghostId = 0; ghostId < _ghostCount; ghostId++)
            {
                result = _gameField.MoveCharacter(_defaultGhostDirection, UniqueTypeIdentifiers.Ghost, ghostId);
                if (result == StepOperationResult.GameOver || result == StepOperationResult.PacmanWins)
                {
                    return;
                }
                if (result == StepOperationResult.PacmanDied)
                {
                    PrintGameField(_gameField._currentLevel);
                    return;
                }
                if (result == StepOperationResult.MoveNotAllowed)
                {
                    _defaultGhostDirection = _gameField.ChangeGhostDirection(ghostId);
                }
            }
            PrintGameField(_gameField._currentLevel);
            System.Threading.Thread.Sleep(_speed);

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
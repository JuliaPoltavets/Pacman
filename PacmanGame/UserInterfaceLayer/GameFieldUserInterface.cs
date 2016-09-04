using System;
using PacmanGame.DataLayer;
using PacmanGame.Model;

namespace PacmanGame.UserInterfaceLayer
{
    public class GameFieldUserInterface
    {
        const int DEFAULT_CONSOLE_WIDTH = 80;
        const int DEFAULT_CONSOLE_HEIGHT = 25;

        private static GameField _gameField;
        private static int _speed;
        private static int _ghostCount;
        private static int _playerCount;
        private static short _levelId;
        private static MoveDirections _defaultGhostDirection = MoveDirections.Up;

        public static void GetInitialSettingsFromUser()
        {
            Console.SetWindowSize(DEFAULT_CONSOLE_WIDTH, DEFAULT_CONSOLE_HEIGHT);
            bool isLevelSelected = false;
            do
            {
                Console.WriteLine("Please select level:");
                string[] levelsList = GetLevelData.GetAvailableLevels();
                for (int levelIndex = 0; levelIndex < levelsList.Length; levelIndex++)
                {
                    Console.WriteLine(levelIndex + ": " + levelsList[levelIndex]);
                }
                string intupLevelId = Console.ReadLine();
                if (short.TryParse(intupLevelId, out _levelId) && _levelId < levelsList.Length)
                {
                    isLevelSelected = true;
                }
            } while (!isLevelSelected);

            bool isGhostCountSelected = false;
            do
            {
                Console.WriteLine("Please select ghosts count (from 0 to 3):");
                string inputGhostCount = Console.ReadLine();
                if (int.TryParse(inputGhostCount, out _ghostCount) && (_ghostCount >= 0) && (_ghostCount <= 3))
                {
                    isGhostCountSelected = true;
                }
            } while (!isGhostCountSelected);

            bool isSpeedSelected = false;
            do
            {
                Console.WriteLine("Please select speed (ms):");
                string inputSpeed = Console.ReadLine();
                if (int.TryParse(inputSpeed, out _speed))
                {
                    isSpeedSelected = true;
                }
            } while (!isGhostCountSelected);
        }

        public static void StartGame()
        {
            _playerCount = 1;
            _gameField = new GameField();
            _gameField.InitLevel(_levelId, _playerCount, _ghostCount);
            Console.SetWindowSize(_gameField._currentLevel._levelWidth + 1, _gameField._currentLevel._levelHeight + 4);
            PrintGameField(_gameField._currentLevel);
            bool isCurrentGameOver = false;
            ConsoleKeyInfo consoleKey = new ConsoleKeyInfo();
            do
            {
                StepOperationResult result;
                while (Console.KeyAvailable == false)
                {
                    if (consoleKey.Key == ConsoleKey.LeftArrow || consoleKey.Key == ConsoleKey.RightArrow ||
                        consoleKey.Key == ConsoleKey.UpArrow || consoleKey.Key == ConsoleKey.DownArrow)
                    {
                        MakeStep(out result, consoleKey.Key);
                    }
                    else
                    {
                        MakeStep(out result);
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
                    MakeStep(out result, consoleKey.Key);
                }
            } while (consoleKey.Key != ConsoleKey.Q && !isCurrentGameOver);

        }

        public static void MakeStep(out StepOperationResult result, ConsoleKey key = ConsoleKey.LeftArrow, int playerId = 0)
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
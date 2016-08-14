using System;
using System.Linq;
using PacmanGame.Model;
using PacmanGame.Utilities;

namespace PacmanGame.DataLayer
{
    public class GetLevelData
    {
        private static string[] levelFilesLocation = new[]
        {
            @"C:\Education\pacman\PacmanGame\Levels\1_level.txt"
        };
        const char _obstacleSymb = '1';
        const char _dotSymb = '0';
        /// <summary>
        /// Method returns the obstacles position for the particular level
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public static Position[] InitObstaclesPositions(short levelId)
        {
            Position[] obstaclesPositions = null;
            string[] levelData = System.IO.File.ReadAllLines(levelFilesLocation[levelId]);
            for(int i = 0; i < levelData.Length; i++)
            {
                char[] getSymbolsSet = levelData[i].Trim().ToCharArray();
                for (int j = 0; j < getSymbolsSet.Length; j++)
                {
                    if (getSymbolsSet[j] == _obstacleSymb)
                    {
                        Position obPos = new Position() {_y = i, _x = j};
                        obstaclesPositions = obstaclesPositions.Add(obPos);
                    }
                }
            }
            return obstaclesPositions;
        }

        public static Position[] InitDotsPositions(short levelId)
        {
            Position[] dotsPositions = null;
            string[] levelData = System.IO.File.ReadAllLines(levelFilesLocation[levelId]);
            for (int i = 0; i < levelData.Length; i++)
            {
                char[] getSymbolsSet = levelData[i].Trim().ToCharArray();
                for (int j = 0; j < getSymbolsSet.Length; j++)
                {
                    if (getSymbolsSet[j] == _dotSymb)
                    {
                        Position dotPos = new Position() { _y = i, _x = j };
                        dotsPositions = dotsPositions.Add(dotPos);
                    }
                }
            }
            return dotsPositions;
        }

        public static bool GetLevelSize(short levelId, out int levelHeight, out int levelWidth )
        {
            bool succeedToGetLevelData = true;
            levelHeight = 0;
            levelWidth = 0;

            string[] levelData = System.IO.File.ReadAllLines(levelFilesLocation[levelId]);
            DataLayerOperationResult verificationResult = CheckIfLevelIsValid(levelData);

            if (verificationResult != DataLayerOperationResult.Successful)
            {
                succeedToGetLevelData = false;
            }
            else
            {
                levelHeight = levelData.Length;
                levelWidth = levelData[0].Trim().Length;
            }
         
            return succeedToGetLevelData;
        }

        private static DataLayerOperationResult CheckIfLevelIsValid(string[] levelData)
        {
            DataLayerOperationResult exception = DataLayerOperationResult.Successful;
            if (levelData == null)
            {
                exception = DataLayerOperationResult.FileWasNotFound;
            }

            if (levelData.Length == 0)
            {
                exception = DataLayerOperationResult.LevelIsEmpty;
            }

            int correctSymbCount = levelData[0].Trim().ToCharArray().Length;
            for (int i = 0; i < levelData.Length; i++)
            {
                int currentSymbCount = levelData[i].Trim().Length;
                if (correctSymbCount != currentSymbCount)
                {
                    exception = DataLayerOperationResult.LevelFormatIsIncorrect;
                    break;
                }
            }
            return exception;
        }
    }
}
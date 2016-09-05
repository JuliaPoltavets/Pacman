using PacmanGame.Model;
using PacmanGame.Utilities;

namespace PacmanGame.DataLayer
{
    public class GetLevelData
    {
        private static string[] levelFilesLocation = new[]
        {
            @"Levels/1_level.txt",
            @"Levels/2_level.txt",
            @"Levels/3_level.txt"
        };
        const char OBSTACLE_SYMBOL = '1';
        const char DOT_SYMBOL = '0';

        /// <summary>
        /// Method returns the obstacles position for the particular level
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public static Position[] InitObstaclesPositions(short levelId)
        {
            return GetPositionsOfGameObjects(OBSTACLE_SYMBOL, levelId);
        }

        public static Position[] InitDotsPositions(short levelId)
        {
            return GetPositionsOfGameObjects(DOT_SYMBOL, levelId);
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

        public static Position[] GetPositionsOfGameObjects(char symbol, short levelId)
        {
            Position[] heroPositions = null;
            string[] levelData = System.IO.File.ReadAllLines(levelFilesLocation[levelId]);
            for (int i = 0; i < levelData.Length; i++)
            {
                char[] getSymbolsSet = levelData[i].Trim().ToCharArray();
                for (int j = 0; j < getSymbolsSet.Length; j++)
                {
                    if (getSymbolsSet[j] == symbol)
                    {
                        Position dotPos = new Position() { _y = i, _x = j };
                        heroPositions = heroPositions.Add(dotPos);
                    }
                }
            }
            return heroPositions;
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

        public static string[] GetAvailableLevels()
        {
            string[] levelsNames = new string[levelFilesLocation.Length];
            for (int i = 0; i<levelFilesLocation.Length;i++)
            {
                string[] splitedPath = levelFilesLocation[i].Split(new[] {'\\', '/','.',':'});
                levelsNames[i] = splitedPath[splitedPath.Length - 2];
            }
            return levelsNames;
        }
    }
}
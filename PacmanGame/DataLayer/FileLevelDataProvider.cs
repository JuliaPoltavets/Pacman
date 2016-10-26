using PacmanGame.Model;
using PacmanGame.Utilities;
using System.Configuration;
using System.IO;

namespace PacmanGame.DataLayer
{
    public class FileLevelDataProvider: LevelDataProvider
    {
        private readonly string _levelFilesPath;
        private readonly string _obstacleSymbol;
        private readonly string _dotSymbol;
        private readonly string[] _levelFilesList;
        private string[] _levelData;


        public FileLevelDataProvider()
        {
            _levelFilesPath = ConfigurationManager.AppSettings["PathToLevelsRepository"];
            _obstacleSymbol = ConfigurationManager.AppSettings["ObstacleSymbol"];
            _dotSymbol = ConfigurationManager.AppSettings["DotSymbol"];
            _levelFilesList = GetAvailableLevels();
        }

        public override DataLayerOperationResult LoadLevel(short levelId)
        {
            DataLayerOperationResult initResult = DataLayerOperationResult.Successful;
            if (_levelFilesList.Length == 0)
            {
                return DataLayerOperationResult.NoAvailableLevels;
            }
            if (_levelFilesList[levelId] == null)
            {
                return DataLayerOperationResult.LevelWasNotFound;
            }
            string[] currentLevelData = System.IO.File.ReadAllLines(_levelFilesList[levelId]);
            initResult = CheckIfLevelIsValid(currentLevelData);
            if (initResult == DataLayerOperationResult.Successful)
            {
                _levelData = currentLevelData;
            }
            return initResult;
        }

        /// <summary>
        /// Method returns the obstacles position for the particular level
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public override DataLayerOperationResult GetObstaclesPositions(short levelId, out Position[] obstaclesPositions)
        {
            DataLayerOperationResult operationResult;
            obstaclesPositions = null;
            if (_levelFilesList[levelId] == null)
            {
                return DataLayerOperationResult.LevelWasNotFound;
            }
            operationResult = CheckIfLevelIsValid(_levelData);
            if (operationResult == DataLayerOperationResult.Successful)
            {
                obstaclesPositions = GetPositionsOfGameObjects(_obstacleSymbol);
            }
            return operationResult;
        }

        public override  DataLayerOperationResult GetDotsPositions(short levelId, out Position[] dotsPositions)
        {
            DataLayerOperationResult operationResult;
            dotsPositions = null;
            if (_levelFilesList[levelId] == null)
            {
                return DataLayerOperationResult.LevelWasNotFound;
            }
            operationResult = CheckIfLevelIsValid(_levelData);
            if (operationResult == DataLayerOperationResult.Successful)
            {
                dotsPositions = GetPositionsOfGameObjects(_dotSymbol);
            }
            return operationResult;
        }

        public override DataLayerOperationResult GetLevelSize(short levelId, out int levelHeight, out int levelWidth )
        {
            DataLayerOperationResult operationResult;
            levelHeight = 0;
            levelWidth = 0;
            if (_levelFilesList[levelId] == null)
            {
                return DataLayerOperationResult.LevelWasNotFound;
            }
            operationResult = CheckIfLevelIsValid(_levelData);
            if (operationResult == DataLayerOperationResult.Successful)
            {
                levelHeight = _levelData.Length;
                levelWidth = _levelData[0].Trim().Length;
            }
            return operationResult;
        }

        private Position[] GetPositionsOfGameObjects(string symbol)
        {
            Position[] heroPositions = null;
            for (int i = 0; i < _levelData.Length; i++)
            {
                var getSymbolsSet = _levelData[i];
                for (int j = 0; j < getSymbolsSet.Length; j++)
                {
                    if (getSymbolsSet[j].ToString() == symbol)
                    {
                        Position dotPos = new Position() { _y = i, _x = j };
                        heroPositions = heroPositions.Add(dotPos);
                    }
                }
            }
            return heroPositions;
        }

        public string[] GetAvailableLevels()
        {
            string[] availableLevelPaths = new string[0];
            if (Directory.Exists(_levelFilesPath))
            {
                availableLevelPaths = Directory.GetFiles(_levelFilesPath);
            }
            return availableLevelPaths;
        }

        private DataLayerOperationResult CheckIfLevelIsValid(string[] levelData)
        {
            if (_levelData == null)
            {
                return DataLayerOperationResult.LevelWasNotInitialized;
            }
            if (levelData.Length == 0)
            {
                return DataLayerOperationResult.LevelIsEmpty;
            }

            int correctSymbCount = levelData[0].Trim().ToCharArray().Length;
            for (int i = 0; i < levelData.Length; i++)
            {
                int currentSymbCount = levelData[i].Trim().Length;
                if (correctSymbCount != currentSymbCount)
                {
                    return DataLayerOperationResult.LevelFormatIsIncorrect;
                }
            }
            return DataLayerOperationResult.Successful;
        }
    }
}
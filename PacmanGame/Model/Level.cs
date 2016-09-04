using System;
using PacmanGame.Utilities;

namespace PacmanGame.Model
{
    public struct Level
    {
        private Dot[] _dots;
        private Obstacle[] _obstacles;
        private static Random _rnd = new Random();

        public Cell[,] _level;
        public int _levelHeight;
        public int _levelWidth;

        public void InitLevel(short levelId)
        {
            InitLevelSize(levelId, out _levelHeight, out _levelWidth);
            InitDotsData(levelId);
            InitObstaclesData(levelId);
            _level = new Cell[_levelHeight, _levelWidth];

            //logic to create all Cell objects with corresponding uniqieIdentifier
            foreach (Dot currentDot in _dots)
            {
                Position dotPos = currentDot._position;
                _level[dotPos._y, dotPos._x] = new Cell()
                {
                    _position = dotPos,
                    _characterId = currentDot._characterId
                };
            }
            foreach (Obstacle currentObstacle in _obstacles)
            {
                Position obstaclePos = currentObstacle._position;
                _level[obstaclePos._y, obstaclePos._x] = new Cell()
                {
                    _position = obstaclePos,
                    _characterId = currentObstacle._characterId
                };
            }
        }
        
        public bool BelongsToLevel(Position position)
        {
            var isInLevelRange = true;
            if ((position._x > _levelWidth - 1) || (position._x < 0))
            {
                isInLevelRange = false;
            }
            if ((position._y > _levelHeight - 1) || (position._y < 0))
            {
                isInLevelRange = false;
            }
            return isInLevelRange;
        }

        public UniqueTypeIdentifiers GetCharacterTypeInCell(Position cellPosition)
        {
            return _level[cellPosition._y, cellPosition._x]._characterId;
        }

        public Position GetRandomDotPosition()
        {
            int randomPosition = _rnd.Next(0, _dots.Length - 1);
            return new Position()
            {
                _y = _dots[randomPosition]._position._y,
                _x = _dots[randomPosition]._position._x
            };
        }

        public bool TryChangeOccupantId(Position cellCoords, UniqueTypeIdentifiers newCharacterId)
        {
            bool newCharacterWasSet = false;
            if (BelongsToLevel(cellCoords))
            {
                if ((GetCharacterTypeInCell(cellCoords) == UniqueTypeIdentifiers.Dot) &&
                    ((newCharacterId & UniqueTypeIdentifiers.Dot) != UniqueTypeIdentifiers.Dot))
                {
                    _dots = _dots.RemoveDotByPosition(cellCoords);
                }
                if (newCharacterId == UniqueTypeIdentifiers.None)
                {
                    newCharacterId = UniqueTypeIdentifiers.EmptyCell;
                }
                _level[cellCoords._y, cellCoords._x]._characterId = newCharacterId;
                newCharacterWasSet = true;
            }
            return newCharacterWasSet;
        }

        public bool CheckAvailableDots()
        {
            return _dots.Length > 0;
        }

        /// <summary>
        /// Gets level heights and width from data layer
        /// </summary>
        /// <param name="levelId">unique level identifier for data layaer</param>
        private void InitLevelSize(short levelId, out int heigth, out int width)
        {
            PacmanGame.DataLayer.GetLevelData.GetLevelSize(levelId, out heigth, out width);
        }

        /// <summary>
        /// Reads data about particular level from the Data layer 
        /// Creats Array of Dots[] _obstacles
        /// Init all instances of Dots in the array
        /// </summary>
        /// <param name="levelId"></param>
        private void InitDotsData(short levelId)
        {
            Position[] dotPositions = PacmanGame.DataLayer.GetLevelData.InitDotsPositions(levelId);
            _dots = new Dot[dotPositions.Length];
            for (int i = 0; i < _dots.Length; i++)
            {
                Dot newDot = new Dot()
                {
                    _characterId = UniqueTypeIdentifiers.Dot,
                    _position = dotPositions[i]
                };
                _dots[i] = newDot;
            }
        }

        /// <summary>
        /// Reads data about particular level from the Data layer 
        /// Creats Array of Obstacle[] _obstacles
        /// Init all instances of Obstacle in the array
        /// </summary>
        /// <param name="levelId">id of the level</param>
        private void InitObstaclesData(short levelId)
        {
            Position[] getObstaclesPositions = PacmanGame.DataLayer.GetLevelData.InitObstaclesPositions(levelId);
            _obstacles = new Obstacle[getObstaclesPositions.Length];
            for (int i = 0; i < _obstacles.Length; i++)
            {
                Obstacle newObstacle = new Obstacle()
                {
                    _characterId = UniqueTypeIdentifiers.Obstacle,
                    _position = getObstaclesPositions[i]
                };
                _obstacles[i] = newObstacle;
            }
        }
    }
}
using PacmanGame.Utilities;

namespace PacmanGame.Model
{
    public struct Level
    {
        public Dot[] _dots;
        public Obstacle[] _obstacles;
        public Cell[,] _level;
        public int _levelHeight;
        public int levelWidth;

        public void InitLevel(short levelId)
        {
            InitLevelSize(levelId);
            InitDotsData(levelId);
            InitObstaclesData(levelId);
            _level = new Cell[height,width];
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
        /// <summary>
        /// Gets level heights and width from data layer
        /// </summary>
        /// <param name="levelId">unique level identifier for data layaer</param>
        private void InitLevelSize(short levelId)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Reads data about particular level from the Data layer 
        /// Creats Array of Dots[] _obstacles
        /// Init all instances of Dots in the array
        /// </summary>
        /// <param name="levelId"></param>
        private void InitDotsData(short levelId)
        {
            Position[] getDotsPositions = PacmanGame.DataLayer.GetLevelData.InitDotsPositions(levelId);
            _dots = new Dot[getDotsPositions.Length];
            for (int i = 0; i < _dots.Length; i++)
            {
                Dot newDot = new Dot()
                {
                    _characterId = UniqueTypeIdentifiers.Dot,
                    _position = getDotsPositions[i]
                };
                _dots = _dots.Add(newDot);
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
            for (int i = 0; i < _dots.Length; i++)
            {
                Obstacle obstacle = new Obstacle()
                {
                    _characterId = UniqueTypeIdentifiers.Obstacle,
                    _position = getObstaclesPositions[i]
                };
                _obstacles = _obstacles.Add(obstacle);
            }
        }

    }
}
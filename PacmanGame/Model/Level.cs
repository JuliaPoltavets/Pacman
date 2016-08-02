namespace PacmanGame.Model
{
    public struct Level
    {
        public Dot[] _dots;
        public Obstacle[] _obstacles;
        public Cell[,] _level;

        public Cell[,] GetLevel(int height, int width, short levelId)
        {
            GetDotsData(levelId);
            GetObstaclesData(levelId);
            //logic to create all Cell objects with corresponding uniqieIdentifier
            return new Cell[height, width];
        }

        /// <summary>
        /// Reads data about particular level from the Data layer 
        /// Creats Array of Dots[] _obstacles
        /// Init all instances of Dots in the array
        /// </summary>
        /// <param name="levelId"></param>
        private void GetDotsData(short levelId)
        {
            _dots = new[]
            {
                new Dot()
            };
        }


        /// <summary>
        /// Reads data about particular level from the Data layer 
        /// Creats Array of Obstacle[] _obstacles
        /// Init all instances of Obstacle in the array
        /// </summary>
        /// <param name="levelId">id of the level</param>
        private void GetObstaclesData(short levelId)
        {
            _obstacles = new[]
            {
                new Obstacle(), 
            };
        }

    }
}
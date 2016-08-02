namespace PacmanGame.Model
{
    public struct GameField
    {
        /// <summary>
        /// height of the playground
        /// </summary>
        public short _height;
        /// <summary>
        /// width of the playground
        /// </summary>
        public short _width;
        /// <summary>
        /// Main characters. Array is selected in case multiplayer extension
        /// </summary>
        public Pacman[] _pacmans;
        /// <summary>
        /// Bots that tend to catch the pacman and eat it
        /// </summary>
        public Ghost[] _ghosts;
        /// <summary>
        /// Before the game level array consists of Dots and Obstacles
        /// Probably will be build based on file data for different levels
        /// </summary>
        public Level[] _levels;
        /// <summary>
        /// height of the playground
        /// </summary>
        public short _currentLevel;

        #region Methods

        /// <summary>
        /// Loads particular level to the _levels array by Id
        /// In case no available levels - pacman won the whole game
        /// </summary>
        /// <param name="levelId"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public void LoadLevel(int levelId, int height, int width)
        {
            // get initial set of dots and obstacles for the level
            //
        }

        #endregion
    }
}
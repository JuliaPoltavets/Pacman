namespace PacmanGame.Model
{
    public struct Pacman
    {
        /// <summary>
        /// All characters have these properties
        /// </summary>
        public Position _position;
        public UniqueTypeIdentifiers _characterId;

        /// <summary>
        /// specific for pacman set of properties
        /// </summary>
        public byte _lives;
        public short _score;

        /// <summary>
        /// initialize new pacman according to the level of the game
        /// in case of pacman deth we need to init pacman with start coords aligned with level using Data layer
        /// </summary>
        /// <param name="levelId"></param>
        /// <returns></returns>
        public Pacman InitPacman(int levelId)
        {
            return new Pacman();
        }

        /// <summary>
        /// Returns current position of the pacman instance
        /// </summary>
        /// <returns></returns>
        public Position GetCurrentPosition()
        {
            return new Position();
        }

        /// <summary>
        /// Returns current position of the pacman instance
        /// </summary>
        /// <returns></returns>
        public int ReduceLives(short count)
        {
            return _lives;
        }

        /// <summary>
        /// Returns current position of the pacman instance
        /// </summary>
        /// <returns></returns>
        public int IncreaseLives(short count)
        {
            return _lives;
        }

    }
}
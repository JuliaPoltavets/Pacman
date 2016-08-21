namespace PacmanGame.Model
{
    public struct Pacman
    {
        /// <summary>
        /// All characters have these properties
        /// </summary>
        public Position _position;
        public Position _defaultPosition;
        public UniqueTypeIdentifiers _characterId;

        /// <summary>
        /// specific for pacman set of properties
        /// </summary>
        public short _lifes;
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
        /// Change current position of Pacman
        /// </summary>
        /// <returns></returns>
        public void SetCurrentPosition(Position newPosition)
        {
            _position = newPosition;
        }

        /// <summary>
        /// Returns current position of the pacman instance
        /// </summary>
        /// <returns></returns>
        public bool TryReduceLifes(short count)
        {
            var isLifeReduced = false;
            if (_lifes - count > 0)
            {
                isLifeReduced = true;
                _lifes -= count;
            }
            return isLifeReduced;
        }

        public int IncreaseLifes(short count)
        {
            return _lifes;
        }

        public void IncreaseScore(short count)
        {
            _score += count;
        }

    }
}
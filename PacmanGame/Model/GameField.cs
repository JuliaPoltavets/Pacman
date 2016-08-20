using System;

namespace PacmanGame.Model
{
    public struct GameField
    {
        const int LIFE_COUNT = 3;
        /// <summary>
        /// Main characters. Array is selected in case multiplayer extension
        /// </summary>
        public Pacman[] _pacmans;
        /// <summary>
        /// Bots that tend to catch the pacman and eat it
        /// </summary>
        public Ghost[] _ghosts;
        /// <summary>
        /// height of the playground
        /// </summary>
        public Level _currentLevel;

        private static Random _rnd = new Random();

        #region Methods

        /// <summary>
        /// Loads particular level to the _levels array by Id
        /// In case no available levels - pacman won the whole game
        /// </summary>
        /// <param name="levelId"></param>
        public void InitLevel(short levelId, int playersCount, int ghostsCount)
        {
            Level level = new Level();
            level.InitLevel(levelId);
            _pacmans = new Pacman[playersCount];
            for (int i = 0; i < playersCount; i++)
            {
                Position pacmanPosition = this.GetRandomDotPosition(level._dots);
                Pacman newPacman = new Pacman()
                {
                    _lives = LIFE_COUNT,
                    _characterId = UniqueTypeIdentifiers.Pacman,
                    _position = pacmanPosition,
                    _defaultPosition = pacmanPosition,
                    _score = 0
                };
                _pacmans[i] = newPacman;
                level._level[newPacman._position._y,newPacman._position._x]._characterId = UniqueTypeIdentifiers.Pacman;
            }
            _ghosts = new Ghost[ghostsCount];
            for (int i = 0; i < ghostsCount; i++)
            {
                Position ghostPosition = this.GetRandomDotPosition(level._dots);
                Ghost newGhost = new Ghost()
                {
                    _characterId = UniqueTypeIdentifiers.Ghost,
                    _position = ghostPosition,
                    _defaultPosition = ghostPosition,
                };
                _ghosts[i] = newGhost;
                level._level[newGhost._position._y, newGhost._position._x]._characterId = UniqueTypeIdentifiers.Ghost;
            }
            _currentLevel = level;
        }

        public void ChangePacmanDirection(MoveDirections direction)
        {
            
        }

        public void MovePacman(MoveDirections direction, int playerId = 0)
        {
            var currentPacman = _pacmans[playerId];
            switch (direction)
            {
                case MoveDirections.Left:
                    var nextPosition = new Position();
                    nextPosition._y = currentPacman._position._y;
                    nextPosition._x = currentPacman._position._x + 1;
                    if (_currentLevel.BelongsToLevel(nextPosition))
                    {
                        UniqueTypeIdentifiers resultantCharacter;
                        var nextCellElements = GetCharacterTypeInCell(nextPosition) | UniqueTypeIdentifiers.Pacman;
                        
                        currentPacman._position.ChangePosition(direction, 1);
                    }

                    break;
                case MoveDirections.Right:
                    break;
                case MoveDirections.Up:
                    break;
                case MoveDirections.Down:
                    break;
                default:
                    break;
            }
        }

        public Position GetRandomDotPosition(Dot[] positionArray)
        {
            int randomPosition = _rnd.Next(0, positionArray.Length - 1);
            return new Position()
            {
                _y = positionArray[randomPosition]._position._y,
                _x = positionArray[randomPosition]._position._x
            };
        }

        private UniqueTypeIdentifiers GetCharacterTypeInCell(Position cellPosition)
        {
            return _currentLevel._level[cellPosition._y, cellPosition._x]._characterId;
        }

        private StepOperationResults ResolveNextCellCharacters(UniqueTypeIdentifiers nextCellChar,
            UniqueTypeIdentifiers activeChar, out UniqueTypeIdentifiers resolvedIds)
        {
            resolvedIds = UniqueTypeIdentifiers.EmptyCell;
            if (activeChar == UniqueTypeIdentifiers.Pacman)
            {
                if ((nextCellChar & UniqueTypeIdentifiers.Ghost) == UniqueTypeIdentifiers.Ghost)
                {
                    //if possible to reduce pacman life du this, call for level init, score was not influenced
                }
                if ((nextCellChar & UniqueTypeIdentifiers.Dot) == UniqueTypeIdentifiers.Dot)
                {
                    //increase the score of pacman, change field to pacman & emptycell
                }
            }
            if (activeChar == UniqueTypeIdentifiers.Ghost)
            {
                if ((nextCellChar & UniqueTypeIdentifiers.Pacman) == UniqueTypeIdentifiers.Pacman)
                {
                    //if possible to reduce pacman life du this, call for level init, score was not influenced
                }
            }
            return StepOperationResults.MoveNotAllowed;
        }

        #endregion
    }
}
using System;

namespace PacmanGame.Model
{
    public struct GameField
    {
        const int LIFE_COUNT = 3;
        const int DOT_VALUE = 3;
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
                Position pacmanPosition = level.GetRandomDotPosition();
                Pacman newPacman = new Pacman()
                {
                    _lifes = LIFE_COUNT,
                    _characterId = UniqueTypeIdentifiers.Pacman,
                    _position = pacmanPosition,
                    _defaultPosition = pacmanPosition,
                    _score = 0
                };
                _pacmans[i] = newPacman;
                level.TryChangeOccupantId(pacmanPosition, UniqueTypeIdentifiers.Pacman);
            }
            _ghosts = new Ghost[ghostsCount];
            for (int i = 0; i < ghostsCount; i++)
            {
                Position ghostPosition = level.GetRandomDotPosition();
                Ghost newGhost = new Ghost()
                {
                    _characterId = UniqueTypeIdentifiers.Ghost,
                    _position = ghostPosition,
                    _defaultPosition = ghostPosition,
                };
                _ghosts[i] = newGhost;
                level.TryChangeOccupantId(ghostPosition, UniqueTypeIdentifiers.Ghost);
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
                    Position nextPosition = new Position
                    {
                        _y = currentPacman._position._y,
                        _x = currentPacman._position._x - 1
                    };
                    if (_currentLevel.BelongsToLevel(nextPosition))
                    {
                        StepOperationResult stepResult = ResolveNextCellCharacters(nextPosition, UniqueTypeIdentifiers.Pacman, currentPacman._position, playerId);
                        
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

        private StepOperationResult ResolveNextCellCharacters(Position nextPosition,
            UniqueTypeIdentifiers activeChar, Position currentPosition, int playerId = 0)
        {
            StepOperationResult revolveOperationResult = StepOperationResult.None;
            UniqueTypeIdentifiers nextCellChar = _currentLevel.GetCharacterTypeInCell(nextPosition);
            UniqueTypeIdentifiers currentCellChar = _currentLevel.GetCharacterTypeInCell(currentPosition);
            if (activeChar == UniqueTypeIdentifiers.Pacman)
            {
                if ((nextCellChar & UniqueTypeIdentifiers.Ghost) == UniqueTypeIdentifiers.Ghost)
                {
                    //if possible to reduce pacman life do this, call for level init, score was not influenced
                    if (_pacmans[playerId].TryReduceLifes(1))
                    {
                        if (_currentLevel.TryChangeOccupantId(nextPosition, nextCellChar | UniqueTypeIdentifiers.Ghost | UniqueTypeIdentifiers.Pacman))
                        {
                            _currentLevel.TryChangeOccupantId(currentPosition, currentCellChar ^ activeChar);
                        }
                        this.RestartLevel();
                        revolveOperationResult = StepOperationResult.PacmanDied;
                    }
                    else
                    {
                        revolveOperationResult = StepOperationResult.GameOver;
                    }
                    
                }
                //if next cell is dot without ghost
                if (((nextCellChar & UniqueTypeIdentifiers.Dot) == UniqueTypeIdentifiers.Dot) && (nextCellChar & UniqueTypeIdentifiers.Ghost) != UniqueTypeIdentifiers.Ghost)
                {
                    //increase the score of pacman,check whether game field has more dots, change field to pacman & emptycell
                    _pacmans[playerId].IncreaseScore(DOT_VALUE);
                    if(_currentLevel.TryChangeOccupantId(nextPosition,
                        UniqueTypeIdentifiers.Pacman | UniqueTypeIdentifiers.EmptyCell))
                    {
                        _currentLevel.TryChangeOccupantId(currentPosition, UniqueTypeIdentifiers.EmptyCell);
                    }

                    revolveOperationResult = StepOperationResult.ValueScored;
                    if (!_currentLevel.CheckAvailableDots())
                    {
                        revolveOperationResult = StepOperationResult.PacmanWins;
                    }
                }
                if ((nextCellChar & UniqueTypeIdentifiers.Obstacle) == UniqueTypeIdentifiers.Obstacle)
                {
                    revolveOperationResult = StepOperationResult.MoveNotAllowed;
                }
                if ((nextCellChar & UniqueTypeIdentifiers.EmptyCell) == UniqueTypeIdentifiers.EmptyCell)
                {
                    if (_currentLevel.TryChangeOccupantId(nextPosition, UniqueTypeIdentifiers.Pacman | UniqueTypeIdentifiers.EmptyCell))
                    {
                        _currentLevel.TryChangeOccupantId(currentPosition, UniqueTypeIdentifiers.EmptyCell);
                    }
                    revolveOperationResult = StepOperationResult.MoveAllowed;
                }
            }
            if (activeChar == UniqueTypeIdentifiers.Ghost)
            {
                if ((nextCellChar & UniqueTypeIdentifiers.Pacman) == UniqueTypeIdentifiers.Pacman)
                {
                    //if possible to reduce pacman life du this, call for level init, score was not influenced
                }
                if ((nextCellChar & UniqueTypeIdentifiers.Ghost) == UniqueTypeIdentifiers.Ghost)
                {
                    //both ghosts possible to be kept together on same cell
                }
                if ((nextCellChar & UniqueTypeIdentifiers.Obstacle) == UniqueTypeIdentifiers.Obstacle)
                {
                    // not allowed to move there
                }
                if ((nextCellChar & UniqueTypeIdentifiers.EmptyCell) == UniqueTypeIdentifiers.EmptyCell)
                {
                    // not allowed to move there
                }
            }

            return StepOperationResult.MoveNotAllowed;
        }

        private void RestartLevel()
        {
            foreach (Pacman player in _pacmans)
            {
                UniqueTypeIdentifiers currentCellOccupantIds = _currentLevel.GetCharacterTypeInCell(player._position);
                if (!player._defaultPosition.Equals(player._position))
                {
                    _currentLevel.TryChangeOccupantId(player._position, currentCellOccupantIds ^ UniqueTypeIdentifiers.Pacman ^ UniqueTypeIdentifiers.Ghost);
                }
                player.SetCurrentPosition(player._defaultPosition);
                _currentLevel.TryChangeOccupantId(player._position, UniqueTypeIdentifiers.Pacman);
            }
            foreach (Ghost gh in _ghosts)
            {
                UniqueTypeIdentifiers currentCellOccupantIds = _currentLevel.GetCharacterTypeInCell(gh._position);
                if (!gh._defaultPosition.Equals(gh._position))
                {
                    _currentLevel.TryChangeOccupantId(gh._position, currentCellOccupantIds ^ UniqueTypeIdentifiers.Pacman ^ UniqueTypeIdentifiers.Ghost);
                }
                gh.SetCurrentPosition(gh._defaultPosition);
                _currentLevel.TryChangeOccupantId(gh._position, UniqueTypeIdentifiers.Ghost);
            }
        }

        private bool CheckIfLevelPassed()
        {
            bool allDotsAreCollected = false;
            return allDotsAreCollected;
        }

        #endregion
    }
}
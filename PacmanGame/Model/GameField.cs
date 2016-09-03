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
        private Pacman[] _pacmans;
        /// <summary>
        /// Bots that tend to catch the pacman and eat it
        /// </summary>
        private Ghost[] _ghosts;
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

        public StepOperationResult MoveCharacter(MoveDirections direction, UniqueTypeIdentifiers charType, int activeCharId)
        {
            StepOperationResult stepResult = StepOperationResult.None;
            Position currentPosition = this.GetCharacterPosition(charType, activeCharId);
            Position nextPosition = this.CalculateNextPosition(direction, currentPosition);
            if (_currentLevel.BelongsToLevel(nextPosition))
            {
                stepResult = ResolveNextCellCharacters(nextPosition, charType, currentPosition);
            }
            switch (stepResult)
            {
                case StepOperationResult.MoveNotAllowed:
                    break;
                case StepOperationResult.PacmanDied:
                    if (_pacmans[activeCharId].TryReduceLifes(1))
                    {
                        this.RestartLevel();
                    }
                    else
                    {
                        stepResult = StepOperationResult.GameOver;
                    }
                    break;
                case StepOperationResult.MoveAllowed:
                    this.MoveActiveCharacter(charType, activeCharId, nextPosition);
                    break;
                case StepOperationResult.ValueScored:
                    this.MoveActiveCharacter(charType, activeCharId, nextPosition);
                    _pacmans[activeCharId].IncreaseScore(DOT_VALUE);
                    break;
                case StepOperationResult.PacmanWins:
                    this.MoveActiveCharacter(charType, activeCharId, nextPosition);
                    break;
                default:
                    break;
            }
            return stepResult;
        }

        #region OldMoveMethod
        //public StepOperationResult MovePacman(MoveDirections direction, int playerId = 0)
        //{
        //    StepOperationResult stepResult = StepOperationResult.None;
        //    Position nextPosition = this.CalculateNextPosition(direction, _pacmans[playerId]._position);

        //    if (_currentLevel.BelongsToLevel(nextPosition))
        //    {
        //        stepResult = ResolveNextCellCharacters(nextPosition, UniqueTypeIdentifiers.Pacman, _pacmans[playerId]._position);
        //    }
        //    switch (stepResult)
        //    {
        //      case StepOperationResult.MoveNotAllowed:
        //            break;
        //      case StepOperationResult.PacmanDied:
        //            if (_pacmans[playerId].TryReduceLifes(1))
        //            {
        //                this.RestartLevel();
        //            }
        //            else
        //            {
        //                stepResult = StepOperationResult.GameOver;
        //            }
        //            break;
        //      case StepOperationResult.MoveAllowed:
        //            _pacmans[playerId]._position = nextPosition;
        //            break;
        //      case StepOperationResult.ValueScored:
        //            _pacmans[playerId]._position = nextPosition;
        //            _pacmans[playerId].IncreaseScore(DOT_VALUE);
        //            break;
        //      case StepOperationResult.PacmanWins:
        //            _pacmans[playerId]._position = nextPosition;
        //            break;
        //        default:
        //            break;
        //    }

        //    return stepResult;
        //}
        #endregion

        private StepOperationResult ResolveNextCellCharacters(Position nextPosition,
            UniqueTypeIdentifiers activeChar, Position currentPosition)
        {
            StepOperationResult revolveOperationResult = StepOperationResult.None;
            UniqueTypeIdentifiers nextCellChar = _currentLevel.GetCharacterTypeInCell(nextPosition);
            UniqueTypeIdentifiers currentCellChar = _currentLevel.GetCharacterTypeInCell(currentPosition);
            if (activeChar == UniqueTypeIdentifiers.Pacman)
            {
                if ((nextCellChar & UniqueTypeIdentifiers.Ghost) == UniqueTypeIdentifiers.Ghost)
                {
                    revolveOperationResult = StepOperationResult.PacmanDied;
                }
                //if next cell is dot without ghost
                if (((nextCellChar & UniqueTypeIdentifiers.Dot) == UniqueTypeIdentifiers.Dot) && (nextCellChar & UniqueTypeIdentifiers.Ghost) != UniqueTypeIdentifiers.Ghost)
                {
                    //increase the score of pacman,check whether game field has more dots, change field to pacman & emptycell
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
                if ((nextCellChar & UniqueTypeIdentifiers.EmptyCell) == UniqueTypeIdentifiers.EmptyCell && (nextCellChar & UniqueTypeIdentifiers.Ghost) != UniqueTypeIdentifiers.Ghost)
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
                    revolveOperationResult = StepOperationResult.PacmanDied;
                }
                if ((nextCellChar & UniqueTypeIdentifiers.Ghost) == UniqueTypeIdentifiers.Ghost)
                {
                    //both ghosts possible to be kept together on same cell
                    if (_currentLevel.TryChangeOccupantId(nextPosition, nextCellChar | UniqueTypeIdentifiers.Ghost))
                    {
                        _currentLevel.TryChangeOccupantId(currentPosition, currentCellChar & ~UniqueTypeIdentifiers.Ghost);
                    }
                    revolveOperationResult = StepOperationResult.MoveAllowed;
                }
                if ((nextCellChar & UniqueTypeIdentifiers.Obstacle) == UniqueTypeIdentifiers.Obstacle)
                {
                    // not allowed to move there
                    revolveOperationResult = StepOperationResult.MoveNotAllowed;
                }
                if ((nextCellChar & UniqueTypeIdentifiers.EmptyCell) == UniqueTypeIdentifiers.EmptyCell 
                    && (nextCellChar & UniqueTypeIdentifiers.Pacman) != UniqueTypeIdentifiers.Pacman)
                {
                    if (_currentLevel.TryChangeOccupantId(nextPosition, nextCellChar | UniqueTypeIdentifiers.Ghost))
                    {
                        _currentLevel.TryChangeOccupantId(currentPosition, currentCellChar & ~UniqueTypeIdentifiers.Ghost);
                    }
                    revolveOperationResult = StepOperationResult.MoveAllowed;
                }
            }

            return revolveOperationResult;
        }

        private void RestartLevel()
        {
            foreach (Pacman player in _pacmans)
            {
                UniqueTypeIdentifiers currentCellOccupantIds = _currentLevel.GetCharacterTypeInCell(player._position);
                if (!player._defaultPosition.Equals(player._position))
                {
                    _currentLevel.TryChangeOccupantId(player._position, currentCellOccupantIds & UniqueTypeIdentifiers.Pacman & UniqueTypeIdentifiers.Ghost);
                }
                player.SetCurrentPosition(player._defaultPosition);
                _currentLevel.TryChangeOccupantId(player._position, UniqueTypeIdentifiers.Pacman);
            }
            foreach (Ghost gh in _ghosts)
            {
                UniqueTypeIdentifiers currentCellOccupantIds = _currentLevel.GetCharacterTypeInCell(gh._position);
                if (!gh._defaultPosition.Equals(gh._position))
                {
                    _currentLevel.TryChangeOccupantId(gh._position, currentCellOccupantIds & UniqueTypeIdentifiers.Pacman & UniqueTypeIdentifiers.Ghost);
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

        private Position CalculateNextPosition(MoveDirections direction, Position currentPosition, int step = 1)
        {
            var nextPosition = new Position();
            switch (direction)
            {
                case MoveDirections.Left:
                    nextPosition = new Position
                    {
                        _y = currentPosition._y,
                        _x = currentPosition._x - step
                    };
                    break;
                case MoveDirections.Right:
                    nextPosition = new Position
                    {
                        _y = currentPosition._y,
                        _x = currentPosition._x + step
                    };
                    break;
                case MoveDirections.Up:
                    nextPosition = new Position
                    {
                        _y = currentPosition._y - step,
                        _x = currentPosition._x
                    };
                    break;
                case MoveDirections.Down:
                    nextPosition = new Position
                    {
                        _y = currentPosition._y + step,
                        _x = currentPosition._x
                    };
                    break;
                default:
                    break;
            }
            return nextPosition;
        }

        /// <summary>
        /// Gets current position of character
        /// </summary>
        /// <param name="charType"></param>
        /// <param name="activeCharId"></param>
        /// <returns></returns>
        private Position GetCharacterPosition(UniqueTypeIdentifiers charType, int activeCharId)
        {
            Position currentPosition;
            if(charType == UniqueTypeIdentifiers.Ghost)
            {
                currentPosition = _ghosts[activeCharId]._position;
            }
            else
            {
                currentPosition = _pacmans[activeCharId]._position;
            }
            return currentPosition;
        }

        /// <summary>
        /// Move active character for this level
        /// </summary>
        /// <param name="charType"></param>
        /// <param name="activeCharId"></param>
        /// <param name="nextPosition"></param>
        private void MoveActiveCharacter(UniqueTypeIdentifiers charType, int activeCharId, Position nextPosition)
        {
            if (charType == UniqueTypeIdentifiers.Ghost)
            {
                _ghosts[activeCharId]._position = nextPosition;
            }
            if (charType == UniqueTypeIdentifiers.Pacman)
            {
                _pacmans[activeCharId]._position = nextPosition;
            }
        }

        #endregion
    }
}
using System;
using PacmanGame.Utilities;

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

        #region PublicMethods

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
                    int pacmanId = this.GetPacmanId(nextPosition);
                    if (_pacmans[pacmanId].TryReduceLifes(1))
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

        private int GetPacmanId(Position currentPosition)
        {
            int id = 0;
            for (int i = 0; i < _pacmans.Length; i++)
            {
                if (_pacmans[i]._position.Equals(currentPosition))
                {
                    id = i;
                }
            }
            return id;
        }

        public MoveDirections ChangeGhostDirection(int ghostId)
        {
            Random rnd = new Random();
            Position currentPosition = this.GetCharacterPosition(UniqueTypeIdentifiers.Ghost, ghostId);
            MoveDirections[] possibleNextMoveDirections = this.CalculatePossibleMovePositions(currentPosition);
            return possibleNextMoveDirections[rnd.Next(0, possibleNextMoveDirections.Length - 1)];
        }

        #endregion

        #region PrivateMethods
        private StepOperationResult ResolveNextCellCharacters(Position nextPosition,
            UniqueTypeIdentifiers activeChar, Position currentPosition)
        {
            StepOperationResult revolveOperationResult = StepOperationResult.None;
            UniqueTypeIdentifiers nextCellChar = _currentLevel.GetCharacterTypeInCell(nextPosition);
            UniqueTypeIdentifiers currentCellChar = _currentLevel.GetCharacterTypeInCell(currentPosition);

            if ((nextCellChar & UniqueTypeIdentifiers.Obstacle) == UniqueTypeIdentifiers.Obstacle)
            {
                revolveOperationResult = StepOperationResult.MoveNotAllowed;
            }

            if ((((nextCellChar | activeChar) & (UniqueTypeIdentifiers.Ghost | UniqueTypeIdentifiers.Dot))
                == (UniqueTypeIdentifiers.Ghost | UniqueTypeIdentifiers.Dot))
                && (nextCellChar & UniqueTypeIdentifiers.Pacman) != UniqueTypeIdentifiers.Pacman)
            {
                if (_currentLevel.TryChangeOccupantId(nextPosition, nextCellChar | UniqueTypeIdentifiers.Ghost))
                {
                    _currentLevel.TryChangeOccupantId(currentPosition, currentCellChar & ~UniqueTypeIdentifiers.Ghost); //Review (cell with problem)
                }
                revolveOperationResult = StepOperationResult.MoveAllowed;
            }
            if ((((nextCellChar | activeChar) & (UniqueTypeIdentifiers.Ghost | UniqueTypeIdentifiers.EmptyCell))
                == (UniqueTypeIdentifiers.Ghost | UniqueTypeIdentifiers.EmptyCell))
                && (nextCellChar & UniqueTypeIdentifiers.Pacman) != UniqueTypeIdentifiers.Pacman)
            {
                if (_currentLevel.TryChangeOccupantId(nextPosition, nextCellChar | UniqueTypeIdentifiers.Ghost))
                {
                    _currentLevel.TryChangeOccupantId(currentPosition, currentCellChar & ~UniqueTypeIdentifiers.Ghost);
                }
                revolveOperationResult = StepOperationResult.MoveAllowed;
            }
            if ((((nextCellChar | activeChar) & (UniqueTypeIdentifiers.Pacman | UniqueTypeIdentifiers.Dot)) 
                == (UniqueTypeIdentifiers.Pacman | UniqueTypeIdentifiers.Dot))
                && (nextCellChar & UniqueTypeIdentifiers.Ghost) != UniqueTypeIdentifiers.Ghost)
            {
                if (_currentLevel.TryChangeOccupantId(nextPosition, UniqueTypeIdentifiers.EmptyCell | UniqueTypeIdentifiers.Pacman))
                {
                    _currentLevel.TryChangeOccupantId(currentPosition, UniqueTypeIdentifiers.EmptyCell);
                }
                revolveOperationResult = StepOperationResult.ValueScored;
                if (!_currentLevel.CheckAvailableDots())
                {
                    revolveOperationResult = StepOperationResult.PacmanWins;
                }
            }
            if ((((nextCellChar | activeChar) & (UniqueTypeIdentifiers.Pacman | UniqueTypeIdentifiers.EmptyCell))
                == (UniqueTypeIdentifiers.Pacman | UniqueTypeIdentifiers.EmptyCell))
                && (nextCellChar & UniqueTypeIdentifiers.Ghost) != UniqueTypeIdentifiers.Ghost)
            {
                if (_currentLevel.TryChangeOccupantId(nextPosition, UniqueTypeIdentifiers.EmptyCell | UniqueTypeIdentifiers.Pacman))
                {
                    _currentLevel.TryChangeOccupantId(currentPosition, UniqueTypeIdentifiers.EmptyCell);
                }
                revolveOperationResult = StepOperationResult.MoveAllowed;
            }

            if (((nextCellChar | activeChar) & (UniqueTypeIdentifiers.Pacman | UniqueTypeIdentifiers.Ghost))
                == (UniqueTypeIdentifiers.Pacman | UniqueTypeIdentifiers.Ghost))
            {
                if (_currentLevel.TryChangeOccupantId(nextPosition, nextCellChar | activeChar))
                {
                    _currentLevel.TryChangeOccupantId(currentPosition, currentCellChar & ~activeChar);
                }
                revolveOperationResult = StepOperationResult.PacmanDied;
            }

            return revolveOperationResult;
        }

        private void RestartLevel()
        {
            for (int pacmanId = 0; pacmanId < _ghosts.Length; pacmanId++)
            {
                UniqueTypeIdentifiers currentCellOccupantIds = _currentLevel.GetCharacterTypeInCell(_pacmans[pacmanId]._position);
                if (!_pacmans[pacmanId]._defaultPosition.Equals(_pacmans[pacmanId]._position))
                {
                    _currentLevel.TryChangeOccupantId(_pacmans[pacmanId]._position, (currentCellOccupantIds & ~UniqueTypeIdentifiers.Pacman) & ~UniqueTypeIdentifiers.Ghost);
                }
                _pacmans[pacmanId]._position = _pacmans[pacmanId]._defaultPosition;
                _currentLevel.TryChangeOccupantId(_pacmans[pacmanId]._position, UniqueTypeIdentifiers.Pacman);
            }
            for (int ghostId = 0; ghostId < _ghosts.Length; ghostId++)
            {
                UniqueTypeIdentifiers currentCellOccupantIds = _currentLevel.GetCharacterTypeInCell(_ghosts[ghostId]._position);
                if (!_ghosts[ghostId]._defaultPosition.Equals(_ghosts[ghostId]._position))
                {
                    _currentLevel.TryChangeOccupantId(_ghosts[ghostId]._position, (currentCellOccupantIds & ~UniqueTypeIdentifiers.Pacman) & ~UniqueTypeIdentifiers.Ghost);
                }
                _ghosts[ghostId]._position = _ghosts[ghostId]._defaultPosition;
                _currentLevel.TryChangeOccupantId(_ghosts[ghostId]._position, UniqueTypeIdentifiers.Ghost);
            }
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
            if (charType == UniqueTypeIdentifiers.Ghost)
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

        private MoveDirections[] CalculatePossibleMovePositions(Position currentPosition)
        {
            MoveDirections[] possibleDirections = null;
            Position leftPosition = this.CalculateNextPosition(MoveDirections.Left, currentPosition);
            if (_currentLevel.BelongsToLevel(leftPosition) && (_currentLevel.GetCharacterTypeInCell(leftPosition) != UniqueTypeIdentifiers.Obstacle))
            {
                possibleDirections = possibleDirections.Add(MoveDirections.Left);
            }
            Position rightPosition = this.CalculateNextPosition(MoveDirections.Right, currentPosition);
            if (_currentLevel.BelongsToLevel(rightPosition) && (_currentLevel.GetCharacterTypeInCell(rightPosition) != UniqueTypeIdentifiers.Obstacle))
            {
                possibleDirections = possibleDirections.Add(MoveDirections.Right);
            }
            Position bottomPosition = this.CalculateNextPosition(MoveDirections.Down, currentPosition);
            if (_currentLevel.BelongsToLevel(bottomPosition) && (_currentLevel.GetCharacterTypeInCell(bottomPosition) != UniqueTypeIdentifiers.Obstacle))
            {
                possibleDirections = possibleDirections.Add(MoveDirections.Down);
            }
            Position topPosition = this.CalculateNextPosition(MoveDirections.Up, currentPosition);
            if (_currentLevel.BelongsToLevel(topPosition) && (_currentLevel.GetCharacterTypeInCell(topPosition) != UniqueTypeIdentifiers.Obstacle))
            {
                possibleDirections = possibleDirections.Add(MoveDirections.Up);
            }
            return possibleDirections;
        }

        #endregion
    }
} 
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
                    var nextPosition = new Position();
                    nextPosition._y = currentPacman._position._y;
                    nextPosition._x = currentPacman._position._x + 1;
                    if (_currentLevel.BelongsToLevel(nextPosition))
                    {
                        UniqueTypeIdentifiers resultantCharacter;
                        
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

        private StepOperationResult ResolveNextCellCharacters(UniqueTypeIdentifiers nextCellChar,
            UniqueTypeIdentifiers activeChar, Position cellCoords, int playerId = 0)
        {
            StepOperationResult revolveOperationResult = StepOperationResult.None;

            if (activeChar == UniqueTypeIdentifiers.Pacman)
            {
                if ((nextCellChar & UniqueTypeIdentifiers.Ghost) == UniqueTypeIdentifiers.Ghost)
                {
                    //if possible to reduce pacman life do this, call for level init, score was not influenced
                    if (_pacmans[playerId].TryReduceLifes(1))
                    {
                        this.RestartLevel();
                        _currentLevel.TryChangeOccupantId(cellCoords, UniqueTypeIdentifiers.Ghost | UniqueTypeIdentifiers.Pacman);
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
                    _currentLevel.TryChangeOccupantId(cellCoords,
                        UniqueTypeIdentifiers.Pacman | UniqueTypeIdentifiers.EmptyCell);
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
                player.SetCurrentPosition(player._defaultPosition);
            }
            foreach (Ghost gh in _ghosts)
            {
                gh.SetCurrentPosition(gh._defaultPosition);
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
using System;

namespace PacmanGame.Model
{
    [Flags]
    public enum UniqueTypeIdentifiers
    {
        None = 0,
        EmptyCell = 1,
        Dot = 2,
        Obstacle = 4,
        Pacman = 8,
        Ghost = 16
    }
}
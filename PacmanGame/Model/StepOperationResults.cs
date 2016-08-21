namespace PacmanGame.Model
{
    public enum StepOperationResult
    {
        None = 0,
        MoveAllowed = 1,
        MoveNotAllowed,
        PacmanDied,
        ValueScored,
        PacmanWins,
        GameOver
    }
}
namespace PacmanGame.DataLayer
{
    public enum DataLayerOperationResult
    {
        None,
        Successful,
        LevelWasNotFound,
        LevelIsEmpty,
        LevelFormatIsIncorrect,
        NoAvailableLevels,
        LevelWasNotInitialized
    }
}
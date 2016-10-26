namespace PacmanGame.DataLayer
{
    public class LevelDataProviderFactory
    {
        public static LevelDataProvider GetLevelDataProvider(string providerName = "txt")
        {
            switch (providerName)
            {
                case "txt":
                    return new FileLevelDataProvider();
                default:
                    return new FileLevelDataProvider();
            }
        }
    }
}
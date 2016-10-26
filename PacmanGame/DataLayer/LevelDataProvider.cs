using PacmanGame.Model;

namespace PacmanGame.DataLayer
{
    public abstract class LevelDataProvider
    {
        public abstract DataLayerOperationResult LoadLevel(short levelId);
        public abstract DataLayerOperationResult GetObstaclesPositions(short levelId, out Position[] obstaclesPositions);
        public abstract DataLayerOperationResult GetDotsPositions(short levelId, out Position[] dotsPositions);
        public abstract DataLayerOperationResult GetLevelSize(short levelId, out int levelHeight, out int levelWidth);

    }
}
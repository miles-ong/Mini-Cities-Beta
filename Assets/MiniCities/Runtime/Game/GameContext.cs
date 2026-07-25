using MiniCities.Runtime.Game.Score;

namespace MiniCities.Runtime.Game
{
    public class GameContext
    {
        public readonly MapManager Map;
        public readonly ScoreManager Score;
        
        public GameContext(MapManager mapManager, ScoreManager scoreManager)
        {
            Map = mapManager;
            Score = scoreManager;
        }
    }
}
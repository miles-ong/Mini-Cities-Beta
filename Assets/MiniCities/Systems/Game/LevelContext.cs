using MiniCities.Runtime.Map;
using MiniCities.Systems.City.Stat;

namespace MiniCities.Systems.Game
{
    public readonly struct LevelContext
    {
        public readonly MapRuntime Map;
        public readonly CityStats TargetStats;
        
        public LevelContext(MapRuntime map, CityStats targetStats)
        {
            Map = map;
            TargetStats = targetStats;
        }
    }
}
using MiniCities.Runtime.Game.BuffSystem;
using MiniCities.Runtime.Game.CitySystem;
using MiniCities.Runtime.Game.MapSystem;

namespace MiniCities.Runtime.Game
{
    public class GameContext
    {
        public readonly MapManager Map;
        public readonly CityManager City;
        public readonly BuffManager Buff;
        
        public GameContext(MapManager map, CityManager city, BuffManager buff)
        {
            Map = map;
            City = city;
            Buff = buff;
        }
    }
}
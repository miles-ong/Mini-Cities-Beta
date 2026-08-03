using MiniCities.Systems.BuffSys;
using MiniCities.Systems.City;
using MiniCities.Systems.Map;

namespace MiniCities.Systems.Game
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
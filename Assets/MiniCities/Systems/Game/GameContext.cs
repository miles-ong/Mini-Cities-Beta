using MiniCities.Systems.BuffSys;
using MiniCities.Systems.CitySys;
using MiniCities.Systems.MapSys;

namespace MiniCities.Systems.GameSys
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
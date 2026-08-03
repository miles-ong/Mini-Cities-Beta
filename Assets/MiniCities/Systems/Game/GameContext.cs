using MiniCities.Core.Random;
using MiniCities.Systems.BuffSys;
using MiniCities.Systems.City;
using MiniCities.Systems.Map;

namespace MiniCities.Systems.Game
{
    public readonly struct GameContext
    {
        public readonly SeededRandom Random;
        public readonly MapManager Map;
        public readonly CityManager City;
        public readonly BuffManager Buff;

        public GameContext(SeededRandom random, MapManager map, CityManager city, BuffManager buff)
        {
            Random = random;
            Map = map;
            City = city;
            Buff = buff;
        }
    }
}
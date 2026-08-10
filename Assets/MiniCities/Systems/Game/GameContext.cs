using MiniCities.Core.Math;
using MiniCities.Runtime.Blueprint;
using MiniCities.Systems.City;
using MiniCities.Systems.Map;

namespace MiniCities.Systems.Game
{
    public readonly struct GameContext
    {
        public readonly SeededRandom Random;
        public readonly MapManager Map;
        public readonly CityManager City;
        public readonly BlueprintFileRuntime File;
        public readonly BlueprintHand Hand;

        public GameContext(
            SeededRandom random, 
            MapManager map, 
            CityManager city, 
            BlueprintFileRuntime file,
            BlueprintHand hand)
        {
            Random = random;
            Map = map;
            City = city;
            File = file;
            Hand = hand;
        }
    }
}
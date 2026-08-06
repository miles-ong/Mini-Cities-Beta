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
        public readonly BlueprintDeckRuntime Deck;
        public readonly BlueprintHand Hand;

        public GameContext(
            SeededRandom random, 
            MapManager map, 
            CityManager city, 
            BlueprintDeckRuntime deck,
            BlueprintHand hand)
        {
            Random = random;
            Map = map;
            City = city;
            Deck = deck;
            Hand = hand;
        }
    }
}
using MiniCities.Core.Math;
using MiniCities.Definitions.Blueprint;
using MiniCities.Definitions.Map;
using MiniCities.Runtime.Blueprint;
using MiniCities.Runtime.Map;
using MiniCities.Systems.City;
using MiniCities.Systems.Map;
using UnityEngine;

namespace MiniCities.Systems.Game
{
    [CreateAssetMenu(fileName = "GameSessionContext", menuName = "Mini Cities/Game Session Context")]
    public class GameSessionContext : ScriptableObject
    {
        public SeededRandom Random { get; private set; }
        public MapPoolRuntime MapPool { get; private set; }
        public BlueprintPoolRuntime BlueprintPool { get; private set; }
        public BlueprintFileRuntime File { get; private set; }
        public BlueprintHand Hand { get; private set; }
        public CityManager City { get; private set; }
        public MapManager Map { get; private set; }

        public int Seed => Random.Seed;

        public void LoadDefinitions(
            int seed,
            MapPoolDefinition mapPoolDefinition,
            BlueprintPoolDefinition blueprintPoolDefinition,
            BlueprintFileDefinition blueprintFileDefinition)
        {
            Random = new SeededRandom(seed);
            MapPool = new MapPoolRuntime(mapPoolDefinition, Random);
            BlueprintPool = new BlueprintPoolRuntime(blueprintPoolDefinition);
            File = new BlueprintFileRuntime(blueprintFileDefinition, Random);
        }

        public void Initialize(
            BlueprintHand blueprintHand,
            CityManager cityManager,
            MapManager mapManager)
        {
            Hand = blueprintHand;
            City = cityManager;
            Map = mapManager;

            Hand.SetFile(File);
            City.PassContext(this);
        }
    }
}
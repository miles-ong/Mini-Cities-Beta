using MiniCities.Definitions.Blueprint;
using MiniCities.Definitions.Map;
using UnityEngine;

namespace MiniCities.Systems.Game
{
    [CreateAssetMenu(fileName = "GameSessionContext", menuName = "Mini Cities/Game Session Context")]
    public class GameSessionContext : ScriptableObject
    {
        public MapPoolDefinition mapPool;
        public BlueprintPoolDefinition blueprintPool;
        public BlueprintFileDefinition blueprintFile;
        public int seed;

        public void Set(
            MapPoolDefinition mapPool,
            BlueprintPoolDefinition blueprintPool,
            BlueprintFileDefinition blueprintFile,
            int seed)
        {
            this.mapPool = mapPool;
            this.blueprintPool = blueprintPool;
            this.blueprintFile = blueprintFile;
            this.seed = seed;
        }
    }
}
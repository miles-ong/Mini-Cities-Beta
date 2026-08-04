using MiniCities.Definitions.Blueprint;
using MiniCities.Runtime.Blueprint;
using MiniCities.Systems.BuffSys;
using UnityEngine;

namespace MiniCities.Runtime.Block
{
    public class BuildingRuntime : BlockRuntime, IBuffReceiver
    {
        public readonly BlueprintRuntime Blueprint;
        
        public BuildingStats BaseStats;
        public BuildingStats TotalStats = new();
        
        public int CurrentVersion { get; private set; }
        public int CachedVersion { get; private set; } = -1;
        
        public BuildingRuntime(BlueprintRuntime blueprint, Vector3Int location) : base(blueprint.Building, location)
        {
            Blueprint = blueprint;
            BaseStats = blueprint.Stats;
        }

        public void MarkDirty() => CurrentVersion++;
        public void MarkClean() => CachedVersion = CurrentVersion;
    }
}
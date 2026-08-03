using MiniCities.Definitions.Block;
using MiniCities.Definitions.Blueprint;
using MiniCities.Systems.BuffSys;
using UnityEngine;

namespace MiniCities.Runtime.Block
{
    public class BuildingRuntime : BlockRuntime, IBuffReceiver
    {
        public BuildingStats BaseStats;
        public BuildingStats TotalStats = new();
        
        public int CurrentVersion { get; private set; }
        public int CachedVersion { get; private set; } = -1;
        
        public BuildingRuntime(BlockDefinition definition, Vector3Int location, BuildingStats baseStats) : base(definition, location)
        {
            BaseStats = baseStats;
        }

        public void MarkDirty() => CurrentVersion++;
        public void MarkClean() => CachedVersion = CurrentVersion;
    }
}
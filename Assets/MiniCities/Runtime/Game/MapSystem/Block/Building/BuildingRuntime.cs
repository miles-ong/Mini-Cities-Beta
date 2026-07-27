using MiniCities.Core.Definitions.Block;
using MiniCities.Runtime.Game.BuffSystem;
using UnityEngine;

namespace MiniCities.Runtime.Game.MapSystem.Block.Building
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
using MiniCities.Definitions.Blueprint.Stat;
using MiniCities.Runtime.Blueprint;
using MiniCities.Systems.City.Buff;
using UnityEngine;

namespace MiniCities.Runtime.Block
{
    public class BuildingRuntime : BlockRuntime, IBuffReceiver, IBuffSource
    {
        public readonly BlueprintRuntime Blueprint;
        public readonly BuildingStats BaseStats;
        public readonly BuildingStats TotalStats;

        public int CurrentVersion { get; private set; }
        public int CachedVersion { get; private set; } = -1;

        public BuildingRuntime(BlueprintRuntime blueprint, Vector3Int location) : base(blueprint.Building, location)
        {
            Blueprint = blueprint;
            BaseStats = blueprint.Stats;
            TotalStats = BaseStats.Clone();
        }

        public void MarkDirty() => CurrentVersion++;
        public void MarkClean() => CachedVersion = CurrentVersion;
    }
}
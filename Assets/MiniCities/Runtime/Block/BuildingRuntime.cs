using System.Collections.Generic;
using MiniCities.Definitions.Ability;
using MiniCities.Definitions.Blueprint.Stat;
using MiniCities.Runtime.Ability;
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
        public int PlacementOrder { get; private set; }

        public BuildingRuntime(BlueprintRuntime blueprint, Vector3Int location) : base(blueprint.Building, location)
        {
            Blueprint = blueprint;
            BaseStats = blueprint.Stats;
            TotalStats = BaseStats.Clone();
        }
        
        public void MarkDirty() => CurrentVersion++;
        public void MarkClean() => CachedVersion = CurrentVersion;
        public void SetPlacementOrder(int placementOrder) => PlacementOrder = placementOrder;

        public IEnumerable<(int index, AbilityRuntime ability)> GetAbilities(AbilityTriggerType type)
        {
            var abilities = Blueprint.Abilities;
            
            for (var i = 0; i < abilities.Count; i++)
                if (abilities[i].Trigger.type == type) yield return (i, abilities[i]);
        }
    }
}
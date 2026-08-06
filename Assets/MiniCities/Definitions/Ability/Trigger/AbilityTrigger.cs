using System;
using JetBrains.Annotations;
using MiniCities.Core.Math;
using MiniCities.Definitions.Block;
using MiniCities.Definitions.Blueprint;
using MiniCities.Runtime.Block;
using MiniCities.Systems.City.Stat;
using UnityEngine;

namespace MiniCities.Definitions.Ability.Trigger
{
    [Serializable]
    public class AbilityTrigger
    {
        public AbilityTriggerType type;

        public bool filterByCategory;
        public BlueprintCategory category;

        public bool filterByBlock;
        [CanBeNull] public BlockDefinition block;

        public CityStat stat;
        public ComparisonOperator comparison;
        public float threshold;

        public bool IsWhenever => type is
            AbilityTriggerType.WheneverBuildingPlaced or
            AbilityTriggerType.WheneverBuildingRemoved or
            AbilityTriggerType.WheneverBuildingMoved or
            AbilityTriggerType.WheneverCityStatReached;

        public bool UsesBuildingFilter => type is
            AbilityTriggerType.WheneverBuildingPlaced or
            AbilityTriggerType.WheneverBuildingRemoved or
            AbilityTriggerType.WheneverBuildingMoved;

        public bool UsesStatFilter => type is
            AbilityTriggerType.WheneverCityStatReached;

        public bool MatchesBuilding(BuildingRuntime triggeringBuilding)
        {
            if (!filterByCategory && !filterByBlock) return true;
            
            if (filterByCategory) return triggeringBuilding.Blueprint.Category == category;
            if (filterByBlock && block != null) return triggeringBuilding.Blueprint.Building == block;

            return false;
        }

        public bool MatchesStat(CityStat triggeringStat, float value)
        {
            if (triggeringStat != stat) return false;

            return comparison switch
            {
                ComparisonOperator.GreaterThanOrEqual => value >= threshold,
                ComparisonOperator.GreaterThan => value > threshold,
                ComparisonOperator.LessThanOrEqual => value <= threshold,
                ComparisonOperator.LessThan => value < threshold,
                ComparisonOperator.Equal => Mathf.Approximately(value, threshold),
                _ => throw new ArgumentOutOfRangeException(nameof(triggeringStat), "Not supported.")
            };
        }

        public AbilityTrigger Clone() => new()
        {
            type = type,
            filterByCategory = filterByCategory,
            category = category,
            filterByBlock = filterByBlock,
            block = block,
            stat = stat,
            comparison = comparison,
            threshold = threshold,
        };
    }
}
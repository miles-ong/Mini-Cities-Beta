using System;
using System.Collections.Generic;
using DG.Tweening;
using MiniCities.Definitions.Ability;
using MiniCities.Definitions.Blueprint.Stat;
using MiniCities.Runtime.Ability;
using MiniCities.Runtime.Blueprint;
using MiniCities.Systems.City.Buff;
using MiniCities.View;
using UnityEngine;

namespace MiniCities.Runtime.Block
{
    public class BuildingRuntime : BlockRuntime, IAbilityActivator, IBuffReceiver, IBuffSource
    {
        public readonly BlueprintRuntime Blueprint;
        public readonly BuildingStats BaseStats;
        public readonly BuildingStats TotalStats;
        
        public int CurrentVersion { get; private set; }
        public int CachedVersion { get; private set; } = -1;

        private int _placementOrder = -1;

        public BuildingRuntime(BlueprintRuntime blueprint, Vector3Int location) : base(blueprint.Building, location)
        {
            Blueprint = blueprint;
            BaseStats = blueprint.Stats;
            TotalStats = BaseStats.Clone();
        }

        public IReadOnlyCollection<(int index, AbilityRuntime ability)> GetAbilities(AbilityTriggerType? type = null)
        {
            var abilities = new List<(int index, AbilityRuntime ability)>();

            for (var i = 0; i < Blueprint.Abilities.Count; i++)
            {
                var ability = Blueprint.Abilities[i];
                
                if (type == null || ability.Trigger.type == type) abilities.Add((i, ability));
            }

            return abilities.AsReadOnly();
        }

        public int GetPriorityValue()
        {
            if (_placementOrder < 0)
                throw new InvalidOperationException(
                    $"{nameof(GetPriorityValue)} called before {nameof(SetPlacementOrder)}!");

            var priority = 0;

            priority += (int)Blueprint.Category * 100;
            priority += _placementOrder;

            return priority;
        }

        public void TelegraphAbilityUsage()
        {
            DOTween.Sequence()
                .Append(View.transform.DOScale(0.9f, 0.2f))
                .Append(View.transform.DOScale(0.8f, 0.2f));
        }
        
        public void MarkDirty() => CurrentVersion++;
        public void MarkClean() => CachedVersion = CurrentVersion;
        public void SetPlacementOrder(int order) => _placementOrder = order;
        public void ShowStatChange(BuildingStat stat, int amount)
        {
            if (View is BuildingView buildingView) buildingView.TelegraphStatChange(stat, amount);
        }
    }
}
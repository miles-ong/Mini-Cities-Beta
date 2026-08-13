using System;
using JetBrains.Annotations;
using MiniCities.Core.Grid;
using MiniCities.Core.Math;
using MiniCities.Definitions.Block;
using MiniCities.Definitions.Blueprint;
using MiniCities.Definitions.Blueprint.Stat;
using MiniCities.Runtime.Block;
using MiniCities.Systems.City.Buff;
using MiniCities.Systems.City.Stat;

namespace MiniCities.Definitions.Ability
{
    public interface IAbilityEffect
    {
        public void Activate(AbilityContext ctx);
        public string Describe();
        public IAbilityEffect Clone();
    }

    [Serializable]
    public class ModifyBuildingBaseStat : IAbilityEffect
    {
        public BuildingStat stat;
        public Operation operation;
        public float amount;

        public void Activate(AbilityContext context)
        {
            if (context.Activator is not BuildingRuntime building)
                throw new InvalidOperationException("Activator must be a building!");
            
            building.BaseStats.Modify(stat, operation, amount);
            building.MarkDirty();
        }

        public string Describe() => $"{operation.ToDescriptionText(amount)} {stat}";
        
        public IAbilityEffect Clone() => new ModifyBuildingBaseStat()
        {
            stat = stat,
            operation = operation,
            amount = amount
        };
    }

    [Serializable]
    public class ModifyCityBaseStat : IAbilityEffect
    {
        public CityStat stat;
        public Operation operation;
        public float amount;

        public void Activate(AbilityContext context) => context.Game.City.ModifyBaseStat(stat, operation, amount);

        public string Describe() => $"{operation.ToDescriptionText(amount)} {stat}";

        public IAbilityEffect Clone() => new ModifyCityBaseStat()
        {
            stat = stat,
            operation = operation,
            amount = amount
        };
    }

    [Serializable]
    public class BuffBuildingStatInRange : IAbilityEffect
    {
        public BuildingStat stat;
        public Operation operation;
        public float amount;
        public GridScope scope;
        public GridLevel level;
        public bool includeSelf;
        public bool buffByCategory;
        public BlueprintCategory targetCategory;
        public bool buffByBuilding;
        public BlockDefinition targetBuilding;

        public void Activate(AbilityContext context)
        {
            if (context.Activator is not BuildingRuntime building) // fix later
                throw new InvalidOperationException("Activator must be a building!");
            
            var locations = context.Game.Map.GetLocationsInScope(building.Location, scope, level, includeSelf);

            foreach (var location in locations)
                context.Game.City.RegisterBuildingBuff(
                    new BuildingBuff(
                        building,
                        stat,
                        operation,
                        amount,
                        location,
                        GetCondition()));
        }

        public string Describe()
        {
            return string.Empty;
        }

        public IAbilityEffect Clone() => new BuffBuildingStatInRange()
        {
            stat = stat,
            operation = operation,
            amount = amount,
            scope = scope,
            level = level,
            includeSelf = includeSelf,
            buffByCategory = buffByCategory,
            targetCategory = targetCategory,
            buffByBuilding = buffByBuilding,
            targetBuilding = targetBuilding
        };

        
        [CanBeNull] private Predicate<BuildingRuntime> GetCondition()
        {
            if (buffByCategory) return b => b.Blueprint.Category == targetCategory;

            if (buffByBuilding) return targetBuilding != null
                ? b => b.Definition == targetBuilding
                : throw new InvalidOperationException($"Building cannot be null when {nameof(buffByBuilding)} is true!");

            return null;
        }
    }

    [Serializable]
    public class DrawBlueprint : IAbilityEffect
    {
        public int amount = 1;

        public void Activate(AbilityContext context)
        {
            switch (amount)
            {
                case <= 0: break;
                case 1: context.Game.Hand.DrawBlueprint(); break;
                case >= 1: context.Game.Hand.DrawMultipleBlueprints(amount); break;
            }
        }

        public string Describe() => $"draw {amount} blueprint{(amount > 1 ? "s" : "")}";

        public IAbilityEffect Clone() => new DrawBlueprint()
        {
            amount = amount
        };
    }
}
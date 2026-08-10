using System;
using System.Linq;
using MiniCities.Core.Grid;
using MiniCities.Definitions.Block;
using MiniCities.Definitions.Blueprint;
using MiniCities.Runtime.Block;

namespace MiniCities.Definitions.Ability
{
    public interface IAbilityCondition
    {
        public bool Evaluate(AbilityContext ctx);
        public string Describe();
        public IAbilityCondition Clone();
    }
    
    [Serializable]
    public sealed class BlueprintCategoryInRange : IAbilityCondition
    {
        public int amount;
        public BlueprintCategory category;
        public GridScope scope;
        public GridLevel level;
        public bool includeSelf;

        public bool Evaluate(AbilityContext ctx) => ctx.Game.Map.GetBlocksInScope<BuildingRuntime>(
            ctx.Owner.Location,
            scope,
            level,
            includeSelf)
            .Count(b => b.Blueprint.Category == category) >= amount;

        public string Describe()
        {
            var subject = amount == 1
                ? $"a {category} building"
                : $"{amount} {category} buildings";

            return $"{scope.ToDescriptionText()} {subject}";
        }

        public IAbilityCondition Clone() => new BlueprintCategoryInRange()
        {
            amount = amount,
            category = category,
            scope = scope,
            level = level,
            includeSelf = includeSelf,
        };
    }

    [Serializable]
    public sealed class BlockGroupInRange : IAbilityCondition
    {
        public int amount;
        public BlockGroup group;
        public GridScope scope;
        public GridLevel level;
        public bool includeSelf;

        public bool Evaluate(AbilityContext ctx) => ctx.Game.Map.GetBlocksInScope(
            ctx.Owner.Location,
            scope,
            level,
            includeSelf).Count(b => b.Definition.group == group) >= amount;

        public string Describe()
        {
            var subject = amount == 1
                ? $"a {group} block"
                : $"{amount} {group} blocks";

            return $"{scope.ToDescriptionText()} {subject}";
        }

        public IAbilityCondition Clone() => new BlockGroupInRange()
        {
            amount = amount,
            group = group,
            scope = scope,
            level = level,
            includeSelf = includeSelf
        };
    }

    [Serializable]
    public sealed class BlockInRange : IAbilityCondition
    {
        public int amount;
        public BlockDefinition block;
        public GridScope scope;
        public GridLevel level;
        public bool includeSelf;

        public bool Evaluate(AbilityContext ctx) => ctx.Game.Map.GetBlocksInScope(
            ctx.Owner.Location,
            scope,
            level,
            includeSelf).Count(b => b.Definition == block) >= amount;
        
        public string Describe()
        {
            var subject = amount == 1
                ? $"a {block.displayName} block"
                : $"{amount} {block.displayName} blocks";

            return $"{scope.ToDescriptionText()} {subject}";
        }

        public IAbilityCondition Clone() => new BlockInRange()
        {
            amount = amount,
            block = block,
            scope = scope,
            level = level,
            includeSelf = includeSelf
        };
    }

    [Serializable]
    public sealed class BlockInRangeOfBuildingPlaced : IAbilityCondition
    {
        public GridScope scope;
        public GridLevel level;

        public bool Evaluate(AbilityContext ctx)
        {
            var buildings =
                ctx.Game.Map.GetBlocksInScope<BuildingRuntime>(ctx.TriggeringBuilding.Location, scope, level, false);

            return buildings.Contains(ctx.Owner);
        }

        public string Describe() => "i dont care rn";

        public IAbilityCondition Clone() => new BlockInRangeOfBuildingPlaced()
        {
            scope = scope,
            level = level,
        };
    }
}
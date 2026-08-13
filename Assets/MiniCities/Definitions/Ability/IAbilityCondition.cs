using System;
using System.Linq;
using MiniCities.Core.Grid;
using MiniCities.Definitions.Block;
using MiniCities.Definitions.Blueprint;
using MiniCities.Runtime.Block;
using UnityEngine;

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
        public bool queryTriggeringBuilding;
        public bool includeSelf;

        public bool Evaluate(AbilityContext context) => context.Game.Map.GetBlocksInScope<BuildingRuntime>(
            AbilityUtility.ParseBuilding(context, queryTriggeringBuilding).Location,
            scope,
            level,
            includeSelf)
            .Count(b => b.Blueprint.Category == category) >= amount;

        public string Describe()
        {
            var subject = amount == 1
                ? $"a {category} building"
                : $"{amount} {category} buildings";
            
            return $"{scope.ToDescriptionText()} {(queryTriggeringBuilding ? subject : "this building")}";
        }

        public IAbilityCondition Clone() => new BlueprintCategoryInRange()
        {
            amount = amount,
            category = category,
            scope = scope,
            level = level,
            queryTriggeringBuilding =  queryTriggeringBuilding,
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
        public bool queryTriggeringBuilding;
        public bool includeSelf;

        public bool Evaluate(AbilityContext context) => context.Game.Map.GetBlocksInScope(
            AbilityUtility.ParseBuilding(context, queryTriggeringBuilding).Location,
            scope,
            level,
            includeSelf)
            .Count(b => b.Definition.group == group) >= amount;

        public string Describe()
        {
            var subject = amount == 1
                ? $"a {group} block"
                : $"{amount} {group} blocks";

            return $"{scope.ToDescriptionText()} {(queryTriggeringBuilding ? subject : "this building")}";
        }

        public IAbilityCondition Clone() => new BlockGroupInRange()
        {
            amount = amount,
            group = group,
            scope = scope,
            level = level,
            queryTriggeringBuilding =  queryTriggeringBuilding,
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
        public bool queryTriggeringBuilding;
        public bool includeSelf;

        public bool Evaluate(AbilityContext ctx) => ctx.Game.Map.GetBlocksInScope(
            AbilityUtility.ParseBuilding(ctx, queryTriggeringBuilding).Location,
            scope,
            level,
            includeSelf)
            .Count(b => b.Definition == block) >= amount;
        
        public string Describe()
        {
            var subject = amount == 1
                ? $"a {block.displayName} block"
                : $"{amount} {block.displayName} blocks";

            return $"{scope.ToDescriptionText()} {(queryTriggeringBuilding ? subject : "this building")}";
        }

        public IAbilityCondition Clone() => new BlockInRange()
        {
            amount = amount,
            block = block,
            scope = scope,
            level = level,
            queryTriggeringBuilding =  queryTriggeringBuilding,
            includeSelf = includeSelf
        };
    }
}
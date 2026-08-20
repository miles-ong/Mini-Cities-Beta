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
    public sealed class BlockInRange : IAbilityCondition
    {
        // Always Drawn
        public int amount;
        public GridScope scope;
        public GridLevel level;
        public bool includeSelf;
        public bool queryTriggeringBuilding;
        
        // Exclusive Options
        public bool checkByCategory;
        public BlueprintCategory targetCategory;
        public bool checkByGroup;
        public BlockGroup targetGroup;
        public bool checkByBlock;
        public BlockDefinition targetBlock;

        public bool Evaluate(AbilityContext context)
        {
            var location = AbilityUtility.ParseBuilding(context, queryTriggeringBuilding).Location;
            var blocks = context.Game.Map.GetBlocksInScope(location, scope, level, includeSelf);

            return blocks.Count(Matches) >= amount;
        }
        
        public string Describe()
        {
            string subject;

            if (checkByCategory) subject = amount == 1 ? $"a {targetCategory} building" : $"{amount} {targetCategory} buildings";
            else if (checkByGroup) subject = amount == 1 ? $"a {targetGroup} block" : $"{amount} {targetGroup} blocks";
            else if (checkByBlock) subject = amount == 1 ? $"a {targetBlock.displayName} block" : $"{amount} {targetBlock.displayName} blocks";
            else subject = amount == 1 ? "a block" : $"{amount} blocks";
            
            return $"{scope.ToDescriptionText()} {(queryTriggeringBuilding ? subject : "this building")}";
        }

        public IAbilityCondition Clone() => (BlockInRange)MemberwiseClone();

        private bool Matches(BlockRuntime b)
        {
            if (checkByCategory) return b.Definition == targetBlock;
            if (checkByGroup) return b.Definition.group == targetGroup;
            if (checkByBlock) return b is BuildingRuntime building && building.Blueprint.Category == targetCategory;

            return true;
        }
    }
}
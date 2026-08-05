using System;
using System.Linq;
using MiniCities.Core.Grid;
using MiniCities.Definitions.Block;
using MiniCities.Runtime.Block;

namespace MiniCities.Definitions.Blueprint.Placement
{
    public interface IPlacementRule
    {
        public bool Evaluate(PlacementContext ctx);
        public string Describe();
    }

    [Serializable]
    public class BlockGroupPlacementRule : IPlacementRule
    {
        public GridScope scope;
        public BlockGroup group;

        public bool Evaluate(PlacementContext ctx)
        {
            var groups = ctx.Map.GetBlocksInScope(ctx.Hit.Cell, scope, GridLevel.Below).Select(b => b.Definition.group);
            
            return groups.Contains(group);
        }
        
        public string Describe() => $"{scope.ToDescriptionText()} {(group == BlockGroup.Building ? "a " : "")}{group}";
    }

    [Serializable]
    public class BlueprintCategoryPlacementRule : IPlacementRule
    {
        public GridScope scope;
        public BlueprintCategory category;

        public bool Evaluate(PlacementContext ctx)
        {
            var categories = ctx.Map.GetBlocksInScope<BuildingRuntime>(ctx.Hit.Cell, scope, GridLevel.Level)
                .Select(b => b.Blueprint.Category);

            return categories.Contains(category);
        }

        public string Describe() => $"{scope.ToDescriptionText()} a {category} building";
    }

    [Serializable]
    public class BlockPlacementRule : IPlacementRule
    {
        public GridScope scope;
        public BlockDefinition block;

        public bool Evaluate(PlacementContext ctx)
        {
            var blocks = ctx.Map.GetBlocksInScope(ctx.Hit.Cell, scope, GridLevel.Level).Select(b => b.Definition);
            
            return blocks.Contains(block);
        }

        public string Describe() => $"{scope} a {block.displayName} block";
    }
}
using System;
using MiniCities.Core.Grid;
using MiniCities.Definitions.Block;

namespace MiniCities.Definitions.Blueprint
{
    public interface IPlacementRule
    {
        public string Describe();
    }

    [Serializable]
    public class BlockGroupPlacementRule : IPlacementRule
    {
        public GridScope scope;
        public BlockGroup group;

        public string Describe() => $"{scope.ToDescriptionText()} {(group == BlockGroup.Building ? "a " : "")}{group}";
    }

    [Serializable]
    public class BlueprintCategoryPlacementRule : IPlacementRule
    {
        public GridScope scope;
        public BlueprintCategory category;

        public string Describe() => $"{scope.ToDescriptionText()} a {category} building";
    }

    [Serializable]
    public class BlockPlacementRule : IPlacementRule
    {
        public GridScope scope;
        public BlockDefinition block;

        public string Describe() => $"{scope} a {block.displayName} block";
    }
}
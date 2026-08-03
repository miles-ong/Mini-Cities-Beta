using System;
using MiniCities.Core.Grid;
using MiniCities.Definitions.Block;

namespace MiniCities.Definitions.Blueprint
{
    public interface IPlacementRule
    {
    }

    [Serializable]
    public struct BlockGroupPlacementRule : IPlacementRule
    {
        public GridScope scope;
        public BlockGroup group;
    }

    [Serializable]
    public struct BlueprintCategoryPlacementRule : IPlacementRule
    {
        public GridScope scope;
        public BlueprintCategory category;
    }

    [Serializable]
    public struct BlockPlacementRule : IPlacementRule
    {
        public GridScope scope;
        public BlockDefinition block;
    }
}
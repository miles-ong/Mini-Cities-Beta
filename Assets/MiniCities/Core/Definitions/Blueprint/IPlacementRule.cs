using System;
using MiniCities.Core.Definitions.Block;
using MiniCities.Core.Grid;

namespace MiniCities.Core.Definitions.Blueprint
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
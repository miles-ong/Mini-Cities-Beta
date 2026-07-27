using MiniCities.Core.Grid;
using UnityEngine;

namespace MiniCities.Runtime.Game.MapSystem.Block
{
    public class BlockInstanceGrid : Grid3D<BlockRuntime>
    {
        protected override Vector3Int GridDimensions { get; set; }
        protected override BlockRuntime[] GridItems { get; set; }

        public Vector3Int Dimensions => GridDimensions;
        public BlockRuntime[] Blocks => GridItems;

        public BlockInstanceGrid(Vector3Int dimensions) => Resize(dimensions);
    }
}
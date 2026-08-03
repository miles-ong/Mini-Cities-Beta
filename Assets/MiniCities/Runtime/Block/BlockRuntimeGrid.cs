using MiniCities.Core.Grid;
using UnityEngine;

namespace MiniCities.Runtime.Block
{
    public class BlockRuntimeGrid : Grid3D<BlockRuntime>
    {
        protected override Vector3Int GridDimensions { get; set; }
        protected override BlockRuntime[] GridItems { get; set; }

        public Vector3Int Dimensions => GridDimensions;
        public BlockRuntime[] Blocks => GridItems;

        public BlockRuntimeGrid(Vector3Int dimensions) => Resize(dimensions);
    }
}
using MiniCities.Core.Grid;
using UnityEngine;

namespace MiniCities.Core.Data.Block.Collections
{
    public class BlockInstanceGrid : Grid3D<BlockInstance>
    {
        protected override Vector3Int GridDimensions { get; set; }
        protected override BlockInstance[] GridItems { get; set; }

        public Vector3Int Dimensions => GridDimensions;
        public BlockInstance[] Blocks => GridItems;

        public BlockInstanceGrid(Vector3Int dimensions) => Resize(dimensions);
    }
}
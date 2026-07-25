using System;
using UnityEngine;

namespace _.Entities.Block.Collections
{
    [Serializable]
    public class BlockDataGrid
    {
        [SerializeField] private Vector3Int dimensions;
        [SerializeField] private BlockData[] blocks = Array.Empty<BlockData>();

        public Vector3Int Dimensions => dimensions;
        public int Width => dimensions.x;
        public int Height => dimensions.y;
        public int Length => dimensions.z;

        public BlockData this[Vector3Int coord]
        {
            get => blocks[GetCheckedIndex(coord)];
            set => blocks[GetCheckedIndex(coord)] = value;
        }

        public BlockDataGrid()
        {
        }

        public BlockDataGrid(Vector3Int dim) => Resize(dim);

        public BlockDataGrid Clone() => new()
        {
            dimensions = dimensions,
            blocks = blocks.Clone() as BlockData[]
        };
        
        public bool InBounds(Vector3Int coord)
            => coord.x >= 0 && coord.x < Width && coord.y >= 0 && coord.y < Height && coord.z >= 0 && coord.z < Length;
        
        public void Resize(Vector3Int newDimensions)
        {
            var resizedBlocks = new BlockData[newDimensions.x * newDimensions.y * newDimensions.z];
            var copyWidth = Mathf.Min(Width, newDimensions.x);
            var copyHeight = Mathf.Min(Height, newDimensions.y);
            var copyLength = Mathf.Min(Length, newDimensions.z);

            for (var y = 0; y < copyHeight; y++)
            for (var z = 0; z < copyLength; z++)
            for (var x = 0; x < copyWidth; x++)
            {
                var coord = new Vector3Int(x, y, z);

                if (blocks.Length > 0)
                    resizedBlocks[GetIndex(coord, newDimensions)] = blocks[GetIndex(coord, dimensions)];
            }

            dimensions = newDimensions;
            blocks = resizedBlocks;
        }
        
        private int GetIndex(Vector3Int coord, Vector3Int dim) 
            => coord.x + (coord.z * dim.x) + (coord.y * dim.x * dim.z);
        
        private int GetCheckedIndex(Vector3Int coord)
        {
            if (!InBounds(coord))
                throw new ArgumentOutOfRangeException(nameof(coord),
                    $"{coord} is out of bounds for dimensions {dimensions}");

            return GetIndex(coord, dimensions);
        }
    }
}
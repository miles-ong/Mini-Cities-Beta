using System;
using MiniCities.Core.Grid;
using UnityEngine;

namespace MiniCities.Core.Data.Block.Collections
{
    [Serializable]
    public class BlockDataGrid : Grid3D<BlockData>
    {
        [SerializeField] private Vector3Int dimensions;
        [SerializeField] private BlockData[] items = Array.Empty<BlockData>();

        public Vector3Int Dimensions => dimensions;
        public BlockData[] Items => items;

        protected override Vector3Int GridDimensions
        {
            get => dimensions;
            set => dimensions = value;
        }

        protected override BlockData[] GridItems
        {
            get => items;
            set => items = value;
        }
    }
}
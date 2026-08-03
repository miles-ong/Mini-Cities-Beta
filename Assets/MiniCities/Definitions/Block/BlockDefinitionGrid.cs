using System;
using MiniCities.Core.Grid;
using UnityEngine;

namespace MiniCities.Definitions.Block
{
    [Serializable]
    public class BlockDefinitionGrid : Grid3D<BlockDefinition>
    {
        [SerializeField] private Vector3Int dimensions;
        [SerializeField] private BlockDefinition[] items = Array.Empty<BlockDefinition>();

        public Vector3Int Dimensions => dimensions;
        public BlockDefinition[] Items => items;

        protected override Vector3Int GridDimensions
        {
            get => dimensions;
            set => dimensions = value;
        }

        protected override BlockDefinition[] GridItems
        {
            get => items;
            set => items = value;
        }
    }
}
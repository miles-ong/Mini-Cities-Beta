using MiniCities.Definitions.Map;
using MiniCities.Runtime.Block;
using UnityEngine;

namespace MiniCities.Runtime.Map
{
    public class MapRuntime
    {
        public MapSize Size;
        public int TerrainFactor;
        public readonly BlockRuntimeGrid Grid;

        public Vector3Int Dimensions => Grid.Dimensions;
        public int Width => Grid.Width;
        public int Height => Grid.Height;
        public int Length => Grid.Length;

        public BlockRuntime this[Vector3Int coord]
        {
            get => Grid[coord];
            set => Grid[coord] = value;
        }
        
        public MapRuntime(MapDefinition definition)
        {
            Size = definition.size;
            TerrainFactor = definition.terrainFactor;
            Grid = new BlockRuntimeGrid(definition.Dimensions + Vector3Int.up); // building layer
            
            for (var y = 0; y < Height - 1; y++)
            for (var z = 0; z < Length; z++)
            for (var x = 0; x < Width; x++)
            {
                var coord = new Vector3Int(x, y, z);
                var data = definition.grid[coord];
                Grid[coord] = new BlockRuntime(data, coord);
            }
        }

        public Vector3 GetWorldCenter()
        {
            var offset = Vector3.zero;

            if (Width % 2 == 0) offset.x = 0.5f;
            if (Length % 2 == 0) offset.z = 0.5f;

            return (Vector3)Dimensions / 2f - offset;
        }
    }
}
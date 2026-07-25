using MiniCities.Core.Data.Block;
using MiniCities.Core.Data.Block.Collections;
using MiniCities.Core.Data.Map.Enums;
using UnityEngine;

namespace MiniCities.Core.Data.Map
{
    public class MapInstance
    {
        public MapSize Size;
        public int TerrainFactor;
        
        private readonly BlockInstanceGrid _grid;

        public Vector3Int Dimensions => _grid.Dimensions;
        public int Width => _grid.Width;
        public int Height => _grid.Height;
        public int Length => _grid.Length;

        public BlockInstance this[Vector3Int coord]
        {
            get => _grid[coord];
            set => _grid[coord] = value;
        }
        
        public MapInstance(MapData data)
        {
            Size = data.size;
            TerrainFactor = data.terrainFactor;
            _grid = new BlockInstanceGrid(data.Dimensions);
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
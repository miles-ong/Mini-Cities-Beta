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
        public readonly BlockInstanceGrid Grid;

        public Vector3Int Dimensions => Grid.Dimensions;
        public int Width => Grid.Width;
        public int Height => Grid.Height;
        public int Length => Grid.Length;

        public BlockInstance this[Vector3Int coord]
        {
            get => Grid[coord];
            set => Grid[coord] = value;
        }
        
        public MapInstance(MapData data)
        {
            Size = data.size;
            TerrainFactor = data.terrainFactor;
            Grid = new BlockInstanceGrid(data.Dimensions);
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
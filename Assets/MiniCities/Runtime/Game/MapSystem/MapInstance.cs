using MiniCities.Core.Definitions.Map;
using MiniCities.Runtime.Game.MapSystem.Block;
using UnityEngine;

namespace MiniCities.Runtime.Game.MapSystem
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
        
        public MapInstance(MapDefinition definition)
        {
            Size = definition.size;
            TerrainFactor = definition.terrainFactor;
            Grid = new BlockInstanceGrid(definition.Dimensions);
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
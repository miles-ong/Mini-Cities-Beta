using _.Entities.Block;
using _.Entities.Block.Collections;
using _.Entities.Map.Enums;
using UnityEngine;

namespace _.Entities.Map
{
    public class MapInstance
    {
        public string Name { get; }
        public MapSize Size { get; }
        public int TerrainFactor { get; private set; }
        public BlockDataGrid Grid { get; }
        
        public int Width => Grid.Width;
        public int Height => Grid.Height;
        public int Length => Grid.Length;

        public BlockData this[Vector3Int coord]
        {
            get => Grid[coord];
            set => Grid[coord] = value;
        }
        
        public MapInstance(MapData data)
        {
            Name = data.name;
            Size = data.size;
            TerrainFactor = data.terrainFactor;
            Grid = data.grid.Clone();
        }

        public Vector3 GetWorldCenter()
        {
            var offset = Vector3.zero;

            if (Width % 2 == 0) offset.x = 0.5f;
            if (Length % 2 == 0) offset.z = 0.5f;

            return (Vector3)Grid.Dimensions / 2f - offset;
        }
    }
}
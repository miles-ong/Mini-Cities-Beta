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
        public BlockGrid Grid { get; }

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
    }
}
using _.Entities.Block;
using _.Entities.Block.Collections;
using _.Entities.Map.Enums;
using UnityEngine;

namespace _.Entities.Map
{
    [CreateAssetMenu(fileName = "NewMapData", menuName = "Mini Cities/Map")]
    public class MapData : ScriptableObject
    {
        public MapSize size;
        public int terrainFactor;
        public BlockGrid grid = new();

        public int Width => grid.Width;
        public int Height => grid.Height;
        public int Length => grid.Length;

        public BlockData this[Vector3Int coord]
        {
            get => grid[coord];
            set => grid[coord] = value;
        }
    }
}
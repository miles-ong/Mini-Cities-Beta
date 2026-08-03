using MiniCities.Core.Definitions.Block;
using UnityEngine;

namespace MiniCities.Core.Definitions.Map
{
    [CreateAssetMenu(fileName = "MAP_", menuName = "Mini Cities/Map")]
    public class MapDefinition : ScriptableObject
    {
        public string displayName = "Unnamed";
        public MapSize size;
        public int terrainFactor;
        public BlockDefinitionGrid grid = new();

        public Vector3Int Dimensions => grid.Dimensions;
        public int Width => grid.Width;
        public int Height => grid.Height;
        public int Length => grid.Length;

        public BlockDefinition this[Vector3Int coord]
        {
            get => grid[coord];
            set => grid[coord] = value;
        }
    }
}
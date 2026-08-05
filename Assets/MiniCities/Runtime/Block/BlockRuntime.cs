using MiniCities.Definitions.Block;
using UnityEngine;

namespace MiniCities.Runtime.Block
{
    public class BlockRuntime
    {
        public BlockDefinition Definition { get; }
        public Vector3Int Location;

        public BlockRuntime(BlockDefinition definition, Vector3Int location)
        {
            Definition = definition;
            Location = location;
        }
    }
}
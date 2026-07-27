using MiniCities.Core.Definitions.Block;
using UnityEngine;

namespace MiniCities.Runtime.Game.MapSystem.Block
{
    public class BlockRuntime
    {
        public BlockDefinition Definition { get; }
        public Vector3Int Location { get; }

        public BlockRuntime(BlockDefinition definition, Vector3Int location)
        {
            Definition = definition;
            Location = location;
        }
    }
}
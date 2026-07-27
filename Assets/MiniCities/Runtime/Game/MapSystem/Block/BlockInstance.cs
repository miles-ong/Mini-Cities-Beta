using MiniCities.Core.Definitions.Block;
using UnityEngine;

namespace MiniCities.Runtime.Game.MapSystem.Block
{
    public class BlockInstance
    {
        public BlockDefinition Definition { get; }
        public Vector3Int Location { get; }

        public BlockInstance(BlockDefinition definition, Vector3Int location)
        {
            Definition = definition;
            Location = location;
        }
    }
}
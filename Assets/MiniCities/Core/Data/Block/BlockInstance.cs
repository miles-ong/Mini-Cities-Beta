using UnityEngine;

namespace MiniCities.Core.Data.Block
{
    public class BlockInstance
    {
        public BlockDefinition Definition { get; }
        public Vector3Int Position { get; }

        public BlockInstance(BlockDefinition definition, Vector3Int position)
        {
            Definition = definition;
            Position = position;
        }
    }
}
using UnityEngine;

namespace MiniCities.Core.Data.Block
{
    public class BlockInstance
    {
        public BlockData Data { get; }
        public Vector3Int Position { get; }

        public BlockInstance(BlockData data, Vector3Int position)
        {
            Data = data;
            Position = position;
        }
    }
}
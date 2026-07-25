using MiniCities.Core.Data.Block.Enums;
using UnityEngine;

namespace MiniCities.Core.Data.Block
{
    [CreateAssetMenu(fileName = "NewBlockData", menuName = "Mini Cities/Block")]
    public class BlockData : ScriptableObject
    {
        public BlockGroup group;
        public GameObject prefab;
    }
}
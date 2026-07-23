using _.Entities.Block.Enums;
using UnityEngine;

namespace _.Entities.Block
{
    [CreateAssetMenu(fileName = "NewBlockData", menuName = "Mini Cities/Block")]
    public class BlockData : ScriptableObject
    {
        public BlockGroup group;
        public GameObject prefab;
    }
}
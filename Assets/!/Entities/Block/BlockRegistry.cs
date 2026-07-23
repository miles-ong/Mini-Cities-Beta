using UnityEngine;

namespace _.Entities.Block
{
    [CreateAssetMenu(fileName = "BlockRegistry", menuName = "Mini Cities/Registry/Block")]
    public class BlockRegistry : ScriptableObject
    {
        public BlockData[] blocks;
    }
}
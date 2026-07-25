using UnityEngine;

namespace MiniCities.Core.Data.Block.Collections
{
    [CreateAssetMenu(fileName = "BlockRegistry", menuName = "Mini Cities/Registry/Block")]
    public class BlockRegistry : ScriptableObject
    {
        public BlockDefinition[] blocks;
    }
}
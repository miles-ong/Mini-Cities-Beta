using UnityEngine;

namespace MiniCities.Core.Definitions.Block
{
    [CreateAssetMenu(fileName = "BlockRegistry", menuName = "Mini Cities/Registry/Block")]
    public class BlockRegistry : ScriptableObject
    {
        public BlockDefinition[] blocks;
    }
}
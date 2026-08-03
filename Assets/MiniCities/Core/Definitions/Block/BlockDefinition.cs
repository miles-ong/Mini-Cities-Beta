using UnityEngine;

namespace MiniCities.Core.Definitions.Block
{
    [CreateAssetMenu(fileName = "BLOCK_", menuName = "Mini Cities/Block")]
    public class BlockDefinition : ScriptableObject
    {
        public string displayName = "Unnamed";
        public BlockGroup group;
        public GameObject prefab;
    }
}
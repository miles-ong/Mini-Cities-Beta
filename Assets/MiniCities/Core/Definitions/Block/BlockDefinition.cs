using UnityEngine;

namespace MiniCities.Core.Definitions.Block
{
    [CreateAssetMenu(fileName = "NewBlockDefinition", menuName = "Mini Cities/Block")]
    public class BlockDefinition : ScriptableObject
    {
        public BlockGroup group;
        public GameObject prefab;
    }
}
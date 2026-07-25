using MiniCities.Core.Data.Block.Enums;
using UnityEngine;

namespace MiniCities.Core.Data.Block
{
    [CreateAssetMenu(fileName = "NewBlockDefinition", menuName = "Mini Cities/Block")]
    public class BlockDefinition : ScriptableObject
    {
        public BlockGroup group;
        public GameObject prefab;
    }
}
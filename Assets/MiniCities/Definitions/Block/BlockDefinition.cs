using MiniCities.View;
using UnityEngine;

namespace MiniCities.Definitions.Block
{
    [CreateAssetMenu(fileName = "BK_", menuName = "Mini Cities/Block")]
    public class BlockDefinition : ScriptableObject
    {
        public string displayName = "Unnamed";
        public BlockGroup group;
        public BlockView view;
    }
}
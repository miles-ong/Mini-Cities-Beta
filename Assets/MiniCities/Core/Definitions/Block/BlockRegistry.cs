using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniCities.Core.Definitions.Block
{
    [CreateAssetMenu(fileName = "BlockRegistry", menuName = "Mini Cities/Registry/Block")]
    public class BlockRegistry : ScriptableObject
    {
        public List<BlockDefinition> blocks = new();
        
        #if UNITY_EDITOR
        private void OnValidate() =>
            blocks.Sort((a, b) => string.Compare(a.displayName, b.displayName, StringComparison.OrdinalIgnoreCase));
        #endif
    }
}
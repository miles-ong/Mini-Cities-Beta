using System;
using System.Collections.Generic;
using MiniCities.View;
using UnityEngine;

namespace MiniCities.Definitions.Block
{
    [CreateAssetMenu(fileName = "BlockRegistry", menuName = "Mini Cities/Registry/Block")]
    public class BlockRegistry : ScriptableObject
    {
        public List<BlockDefinition> blocks = new();
        
        private NullBuildingView _nullBuildingView;

        public NullBuildingView NullBuildingView => _nullBuildingView;
        
        #if UNITY_EDITOR
        private void OnValidate() =>
            blocks.Sort((a, b) => string.Compare(a.displayName, b.displayName, StringComparison.OrdinalIgnoreCase));
        #endif
    }
}
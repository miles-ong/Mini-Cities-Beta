using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniCities.Definitions.Blueprint
{
    [CreateAssetMenu(fileName = "BlueprintFileRegistry", menuName = "Mini Cities/Registry/Blueprint File")]
    public class BlueprintFileRegistry : ScriptableObject
    {
        public List<BlueprintFileDefinition> blueprintFiles;
        
        #if UNITY_EDITOR
        
        private void OnValidate() =>
            blueprintFiles.Sort((a, b) =>
                string.Compare(a.displayName, b.displayName, StringComparison.OrdinalIgnoreCase));
        
        #endif
    }
}
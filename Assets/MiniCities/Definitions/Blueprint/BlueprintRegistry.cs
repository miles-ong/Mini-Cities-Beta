using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniCities.Definitions.Blueprint
{
    [CreateAssetMenu(fileName = "BlueprintRegistry", menuName = "Mini Cities/Registry/Blueprint")]
    public class BlueprintRegistry : ScriptableObject
    {
        public List<BlueprintDefinition> blueprints = new();
        
        #if UNITY_EDITOR
        private void OnValidate() =>
            blueprints.Sort((a, b) => string.Compare(a.displayName, b.displayName, StringComparison.OrdinalIgnoreCase));
        #endif
    }
}
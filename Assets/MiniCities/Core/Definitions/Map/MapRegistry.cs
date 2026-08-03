using System;
using System.Collections.Generic;
using UnityEngine;

namespace MiniCities.Core.Definitions.Map
{
    [CreateAssetMenu(fileName = "MapRegistry", menuName = "Mini Cities/Registry/Map")]
    public class MapRegistry : ScriptableObject
    {
        public List<MapDefinition> maps = new();
        
        #if UNITY_EDITOR
        private void OnValidate() =>
            maps.Sort((a, b) => string.Compare(a.displayName, b.displayName, StringComparison.OrdinalIgnoreCase));
        #endif
    }
}
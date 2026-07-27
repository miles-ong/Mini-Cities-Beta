using UnityEngine;

namespace MiniCities.Core.Definitions.Map
{
    [CreateAssetMenu(fileName = "NewMapRegistry", menuName = "Mini Cities/Registry/Map")]
    public class MapRegistry : ScriptableObject
    {
        public MapDefinition[] maps;
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MiniCities.Definitions.Map
{
    [CreateAssetMenu(fileName = "MP_", menuName = "Mini Cities/Pool/Map")]
    public class MapPoolDefinition : ScriptableObject
    {
        public List<MapDefinition> maps = new();

        public void Add(MapDefinition map)
        {
            if (!maps.Contains(map)) maps.Add(map);
        }

        public void Remove(MapDefinition map) => maps.Remove(map);
        public bool Contains(MapDefinition map) => maps.Contains(map);

        public IReadOnlyList<MapDefinition> GetBySize(MapSize size) => 
            maps.Where(m => m.size == size).ToList();
    }
}
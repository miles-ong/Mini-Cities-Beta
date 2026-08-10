using System.Collections.Generic;
using System.Linq;
using MiniCities.Core.Pool;
using UnityEngine;

namespace MiniCities.Definitions.Map
{
    [CreateAssetMenu(fileName = "MP_", menuName = "Mini Cities/Pool/Map")]
    public class MapPoolDefinition : ScriptableObject
    {
        public Pool<MapDefinition> maps = new();

        public IReadOnlyList<MapDefinition> GetBySize(MapSize size) => 
            maps.Items.Where(m => m.size == size).ToList();
    }
}
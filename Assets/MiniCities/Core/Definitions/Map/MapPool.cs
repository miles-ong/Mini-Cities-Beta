using System.Collections.Generic;
using System.Linq;
using MiniCities.Core.Pool;
using UnityEngine;

namespace MiniCities.Core.Definitions.Map
{
    [CreateAssetMenu(fileName = "NewMapPool", menuName = "Mini Cities/Pool/Map")]
    public class MapPool : ScriptableObject
    {
        public Pool<MapDefinition> maps = new();

        public IReadOnlyList<MapDefinition> GetBySize(MapSize size) => 
            maps.Items.Where(m => m.size == size).ToList();
    }
}
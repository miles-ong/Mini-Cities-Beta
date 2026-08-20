using System.Collections.Generic;
using System.Linq;
using MiniCities.Core.Math;
using MiniCities.Definitions.Map;
using UnityEngine;

namespace MiniCities.Runtime.Map
{
    public class MapPoolRuntime
    {
        private List<MapDefinition> _maps;
        private SeededRandom _random;
        
        private List<MapDefinition> _activeMaps = new();
        private MapSize _currentSize;

        public MapPoolRuntime(MapPoolDefinition definition, SeededRandom random)
        {
            _maps = new List<MapDefinition>(definition.maps);
            _random = random;
        }

        public void SetTargetMapSize(MapSize size)
        {
            _activeMaps.Clear();

            foreach (var map in _maps.Where(m => m.size == size))
                _activeMaps.Add(map);

            _currentSize = size;
        }

        public MapDefinition GetRandomMap()
        {
            if (_activeMaps.Count == 0) SetTargetMapSize(_currentSize);

            return GetRandomActiveMap();
        }

        public List<MapDefinition> GetRandomMaps(int count)
        {
            var maps = new List<MapDefinition>();
            
            for (var i = 0; i < count; i++)
            {
                if (_activeMaps.Count == 0)
                {
                    SetTargetMapSize(_currentSize);

                    if (_activeMaps.Count <= maps.Count) continue;
                    
                    foreach (var map in maps)
                        _activeMaps.Remove(map);
                }
                
                maps.Add(GetRandomActiveMap());
            }

            return maps;
        }

        private MapDefinition GetRandomActiveMap()
        {
            var index = _random.Range(0, _activeMaps.Count);
            Debug.Log($"Count: {_activeMaps.Count}");
            var map = _activeMaps[index];
            _activeMaps.RemoveAt(index);
            
            return map;
        }
    }
}
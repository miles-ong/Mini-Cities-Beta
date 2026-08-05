using System.Collections.Generic;
using JetBrains.Annotations;
using MiniCities.Core.Grid;
using MiniCities.Definitions.Map;
using MiniCities.Runtime.Block;
using MiniCities.Runtime.Map;
using MiniCities.Systems.Cam;
using UnityEngine;

namespace MiniCities.Systems.Map
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private CameraController cameraController;
        
        [SerializeField] private MapRenderer renderer;
        
        private MapRuntime _currentMap;

        public void LoadMap(MapDefinition definition)
        {
            renderer.Clear();
            _currentMap = new MapRuntime(definition);
            cameraController.SetAnchor(_currentMap.GetWorldCenter());
            
            for (var y = 0; y < _currentMap.Height; y++) 
            for (var z = 0; z < _currentMap.Length; z++)
            for (var x = 0; x < _currentMap.Width; x++)
            {
                var coord = new Vector3Int(x, y, z);
                var data = definition.grid[coord];
                var block = new BlockRuntime(data, coord);
                
                _currentMap[coord] = block;
                renderer.RenderBlock(coord, block);
            }
        }
        
        public void UnloadMap()
        {
            renderer.Clear();
            _currentMap = null;
        }

        public void SetBlock(Vector3Int coord, [CanBeNull] BlockRuntime block)
        {
            _currentMap[coord] = block;
            renderer.UpdateBlock(coord, block);
        }

        public HashSet<BlockRuntime> GetBlocksInScope(Vector3Int coord, GridScope scope, GridLevel level, bool includeSelf = false) =>
            _currentMap.Grid.GetItemsInScope(coord, scope, level, includeSelf);

        public HashSet<T> GetBlocksInScope<T>(
            Vector3Int coord,
            GridScope scope,
            GridLevel level,
            bool includeSelf = false)
            where T : BlockRuntime
        {
            var result = new HashSet<T>();

            foreach (var b in GetBlocksInScope(coord, scope, level, includeSelf))
            {
                if (b is T typed) result.Add(typed);
            }

            return result;
        }
    }
}
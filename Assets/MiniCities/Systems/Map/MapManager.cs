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
            _currentMap = new MapRuntime(definition);
            renderer.Render(_currentMap, definition.grid);
            cameraController.SetAnchor(_currentMap.GetWorldCenter());
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

        public HashSet<BlockRuntime> GetBlocksInScope(Vector3Int coord, GridScope scope) =>
            _currentMap.Grid.GetItemsInScope(coord, scope);
    }
}
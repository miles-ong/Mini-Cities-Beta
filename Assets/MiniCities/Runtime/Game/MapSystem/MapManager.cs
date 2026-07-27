using System.Collections.Generic;
using JetBrains.Annotations;
using MiniCities.Core.Definitions.Map;
using MiniCities.Core.Grid;
using MiniCities.Runtime.Game.MapSystem.Block;
using MiniCities.Runtime.View.Cam;
using UnityEngine;

namespace MiniCities.Runtime.Game.MapSystem
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private CameraController cameraController;
        
        [SerializeField] private MapRenderer renderer;
        
        private MapInstance _currentMap;

        public void LoadMap(MapDefinition definition)
        {
            _currentMap = new MapInstance(definition);
            renderer.Render(_currentMap, definition.grid);
            cameraController.SetAnchor(_currentMap.GetWorldCenter());
        }

        public void UnloadMap()
        {
            renderer.Clear();
            _currentMap = null;
        }

        public void SetBlock(Vector3Int coord, [CanBeNull] BlockInstance block)
        {
            _currentMap[coord] = block;
            renderer.UpdateBlock(coord, block);
        }

        public HashSet<BlockInstance> GetBlocksInScope(Vector3Int coord, GridScope scope) =>
            _currentMap.Grid.GetItemsInScope(coord, scope);
    }
}
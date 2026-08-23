using System.Collections.Generic;
using JetBrains.Annotations;
using MiniCities.Core.Event;
using MiniCities.Core.Grid;
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

        private void Start()
        {
            GameEventBus<BlueprintUsedEvent>.OnEvent += OnBlueprintUsed;
        }

        public void LoadMap(MapRuntime map)
        {
            renderer.Clear();
            renderer.RenderMap(map);
            cameraController.SetAnchor(map.GetWorldCenter());
            _currentMap = map;
        }
        
        public void UnloadMap()
        {
            renderer.Clear();
            _currentMap = null;
        }

        public BlockRuntime GetBlock(Vector3Int coord) => _currentMap[coord];

        public void SetBlock(Vector3Int coord, [CanBeNull] BlockRuntime block)
        {
            _currentMap[coord] = block;

            if (block != null) block.Location = coord;
            
            renderer.UpdateBlock(coord, block);
        }

        public List<Vector3Int> GetLocationsInScope(
            Vector3Int coord, 
            GridScope scope, 
            GridLevel level,
            bool includeSelf = false) =>
            _currentMap.Grid.GetLocationsInScope(coord, scope, level, includeSelf);

        public HashSet<BlockRuntime> GetBlocksInScope(
            Vector3Int coord, 
            GridScope scope, 
            GridLevel level, 
            bool includeSelf = false) =>
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

        private void OnBlueprintUsed(BlueprintUsedEvent e)
        {
            var building = new BuildingRuntime(e.Blueprint, e.Location);
            
            SetBlock(e.Location, building);
            
            GameEventBus<BuildingPlacedEvent>.Fire(new BuildingPlacedEvent(building));
        }
    }
}
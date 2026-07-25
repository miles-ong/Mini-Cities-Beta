using _.Entities.Block;
using _.Entities.Map;
using _.Systems.Camera;
using JetBrains.Annotations;
using UnityEngine;

namespace _.Systems.Map
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private CameraController cameraController;
        
        [SerializeField] private MapRenderer renderer;
        
        private MapInstance _currentMap;

        public void LoadMap(MapData data)
        {
            _currentMap = data.ToInstance();
            renderer.Render(_currentMap);
            cameraController.SetAnchor(_currentMap.GetWorldCenter());
        }

        public void UnloadMap()
        {
            renderer.Clear();
            _currentMap = null;
        }

        public void SetBlock(Vector3Int coord, [CanBeNull] BlockData block)
        {
            _currentMap.Grid[coord] = block;
            renderer.UpdateBlock(coord, block);
        }
    }
}
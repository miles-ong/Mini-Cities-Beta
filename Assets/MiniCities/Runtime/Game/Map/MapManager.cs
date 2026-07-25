using JetBrains.Annotations;
using MiniCities.Core.Data.Block;
using MiniCities.Core.Data.Map;
using MiniCities.Runtime.View.Cam;
using UnityEngine;

namespace MiniCities.Runtime.Game
{
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private CameraController cameraController;
        
        [SerializeField] private MapRenderer renderer;
        
        private MapInstance _currentMap;

        public void LoadMap(MapData data)
        {
            _currentMap = new MapInstance(data);
            renderer.Render(_currentMap, data.grid);
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
    }
}
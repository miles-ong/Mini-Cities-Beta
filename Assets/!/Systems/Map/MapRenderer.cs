using System.Collections.Generic;
using _.Entities.Block;
using _.Entities.Map;
using JetBrains.Annotations;
using UnityEngine;

namespace _.Systems.Map
{
    public class MapRenderer : MonoBehaviour
    {
        private readonly Dictionary<Vector3Int, GameObject> _views = new();

        public void Render(MapInstance map)
        {
            Clear();
            
            for (var y = 0; y < map.Height; y++)
            for (var z = 0; z < map.Length; z++)
            for (var x = 0; x < map.Width; x++)
            {
                var coord = new Vector3Int(x, y, z);
                var block = map[coord];

                RenderBlock(coord, block);
            }
        }

        public void UpdateBlock(Vector3Int coord, [CanBeNull] BlockData block)
        {
            if (_views.TryGetValue(coord, out var existing))
            {
                Destroy(existing);
                _views.Remove(coord);
            }

            RenderBlock(coord, block);
        }

        public void Clear()
        {
            foreach (var v in _views.Values) Destroy(v);
            
            _views.Clear();
        }

        private void RenderBlock(Vector3Int coord, [CanBeNull] BlockData block)
        {
            if (block is null) return;
            
            var gameObj = Instantiate(block.prefab, coord, Quaternion.identity, transform);
            _views[coord] = gameObj;
        }
    }
}
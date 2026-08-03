using System.Collections.Generic;
using JetBrains.Annotations;
using MiniCities.Definitions.Block;
using MiniCities.Runtime.Block;
using MiniCities.Runtime.Map;
using UnityEngine;

namespace MiniCities.Systems.Map
{
    public class MapRenderer : MonoBehaviour
    {
        private readonly Dictionary<Vector3Int, GameObject> _views = new();

        public void Render(MapRuntime map, BlockDefinitionGrid grid)
        {
            Clear();
            
            for (var y = 0; y < map.Height; y++)
            for (var z = 0; z < map.Length; z++)
            for (var x = 0; x < map.Width; x++)
            {
                var coord = new Vector3Int(x, y, z);
                var data = grid[coord];
                var inst = new BlockRuntime(data, coord);

                RenderBlock(coord, inst);
            }
        }

        public void UpdateBlock(Vector3Int coord, [CanBeNull] BlockRuntime block)
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

        private void RenderBlock(Vector3Int coord, [CanBeNull] BlockRuntime block)
        {
            if (block is null) return;
            
            var gameObj = Instantiate(block.Definition.prefab, coord, Quaternion.identity, transform);
            _views[coord] = gameObj;
        }
    }
}
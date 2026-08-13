using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MiniCities.Runtime.Block;
using MiniCities.Runtime.Map;
using MiniCities.View;
using UnityEngine;

namespace MiniCities.Systems.Map
{
    public class MapRenderer : MonoBehaviour
    {
        [SerializeField] private NullBuildingView nullBuildingView;
        
        private readonly Dictionary<Vector3Int, BlockView> _views = new();

        public void RenderMap(MapRuntime map)
        {
            Clear();
            
            for (var y = 0; y < map.Height; y++)
            for (var z = 0; z < map.Length; z++)
            for (var x = 0; x < map.Width; x++)
            {
                var coord = new Vector3Int(x, y, z);
                RenderBlock(coord, map[coord]);
            }
        }
        
        public void RenderBlock(Vector3Int coord, [CanBeNull] BlockRuntime block)
        {
            if (block is null) return;
            
            _views[coord] = CreateBlockView(block, coord);
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

        private BlockView CreateBlockView(BlockRuntime block, Vector3Int position)
        {
            BlockView view;
            
            if (block.Definition.view != null)
            {
                view = Instantiate(block.Definition.view, position, Quaternion.identity, transform);
            }
            else if (block is BuildingRuntime building)
            {
                var nullView = Instantiate(nullBuildingView, position, Quaternion.identity, transform);
                nullView.Set(building.Blueprint);
                view = nullView;
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(block), "Failed to find null prefab.");
            }
            
            view.SetLayer(LayerMask.NameToLayer("Placeable"));
            block.SetView(view);
            
            return view;
        }
    }
}
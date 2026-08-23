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
            foreach (var block in map.Grid.Blocks) RenderBlock(block);
        }

        public void UpdateBlock(Vector3Int coord, [CanBeNull] BlockRuntime block)
        {
            if (_views.TryGetValue(coord, out var existing))
            {
                Destroy(existing);
                _views.Remove(coord);
            }

            RenderBlock(block);
        }

        public void Clear()
        {
            foreach (var v in _views.Values) Destroy(v);
            
            _views.Clear();
        }
        
        private void RenderBlock([CanBeNull] BlockRuntime block)
        {
            if (block is null) return;
            
            _views[block.Location] = CreateBlockView(block);
        }

        private BlockView CreateBlockView(BlockRuntime block)
        {
            BlockView view;
            
            if (block.Definition.view != null)
            {
                view = Instantiate(block.Definition.view, block.Location, Quaternion.identity, transform);
            }
            else if (block is BuildingRuntime building)
            {
                var nullView = Instantiate(nullBuildingView, block.Location, Quaternion.identity, transform);
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
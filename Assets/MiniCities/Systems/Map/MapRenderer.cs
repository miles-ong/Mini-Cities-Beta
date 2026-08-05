using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using MiniCities.Runtime.Block;
using MiniCities.View;
using UnityEngine;

namespace MiniCities.Systems.Map
{
    public class MapRenderer : MonoBehaviour
    {
        [SerializeField] private NullBuildingView nullBuildingView;
        
        private readonly Dictionary<Vector3Int, BlockView> _views = new();
        
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
            if (block.Definition.view != null)
                return Instantiate(block.Definition.view, position, Quaternion.identity, transform);

            if (block is BuildingRuntime building)
            {
                var nullView = Instantiate(nullBuildingView, position, Quaternion.identity, transform);
                nullView.Set(building.Blueprint);

                return nullView;
            }

            throw new MissingFieldException(nameof(block), "Failed to find null prefab.");
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

namespace MiniCities.Core.Grid
{
    public class GridRaycaster
    {
        private readonly Camera _camera;

        public GridRaycaster(Camera camera) => _camera = camera;

        public bool TryGetGridHit(LayerMask mask, out GridHit gridHit)
        {
            var ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out var hit, 100f, mask))
            {
                var cell = GridTransform.WorldToCell(hit.point);
                gridHit = new GridHit(cell, hit.point, hit.normal);
                
                return true;
            }

            gridHit = default;
            
            return false;
        }
    }
}
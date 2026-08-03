using MiniCities.Core.Grid;
using UnityEngine;

namespace MiniCities.Systems.Tracker
{
    public class PlacementTracker : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private LayerMask placementMask;
        [SerializeField] private Transform previewer;
        
        private GridRaycaster _raycaster;

        private void Start() => _raycaster = new GridRaycaster(camera);

        private void Update()
        {
            if (_raycaster.TryGetGridHit(placementMask, out var hit) && hit.IsTop)
            {
                previewer.position = GridTransform.CellToWorld(hit.Cell) + Vector3.up * 0.5f;
            }
        }
    }
}
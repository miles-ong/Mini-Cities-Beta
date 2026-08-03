using MiniCities.Core.Grid;
using MiniCities.Core.Raycast;
using UnityEngine;

namespace MiniCities.Runtime.Game.Tracker
{
    public class PlacementTracker : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private LayerMask placementMask;
        [SerializeField] private Transform previewer;
        
        private RaycastService _raycaster;

        private void Start() => _raycaster = new RaycastService(camera);

        private void Update()
        {
            if (_raycaster.TryGetGridHit(placementMask, out var hit) && hit.IsTop)
            {
                previewer.position = GridTransform.CellToWorld(hit.Cell) + Vector3.up * 0.5f;
            }
        }
    }
}
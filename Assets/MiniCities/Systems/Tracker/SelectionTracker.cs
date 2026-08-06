using MiniCities.Core.Grid;
using UnityEngine;

namespace MiniCities.Systems.Tracker
{
    public class SelectionTracker : MonoBehaviour
    {
        [SerializeField] private LayerMask placementMask;

        private GridRaycaster _raycaster;

        public void Initialize(GridRaycaster raycaster)
        {
            _raycaster = raycaster;
        }

        public void Begin()
        {
            enabled = true;
        }

        public void End()
        {
            enabled = false;
        }
    }
}
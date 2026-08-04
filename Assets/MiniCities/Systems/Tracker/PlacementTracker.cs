using System.Collections.Generic;
using MiniCities.Core.Grid;
using MiniCities.Definitions.Blueprint;
using MiniCities.Runtime.Blueprint;
using MiniCities.View;
using UnityEngine;

namespace MiniCities.Systems.Tracker
{
    public class PlacementTracker : MonoBehaviour
    {
        [SerializeField] private NullBuildingView nullBuildingView;
        [SerializeField] private LayerMask mask;
        
        private GridRaycaster _raycaster;
        private GameObject _preview;
        private Transform _transform;
        private Renderer[] _renderers;
        private List<IPlacementRule> _rules;

        private bool _lastHit = false;

        public void SetRaycaster(GridRaycaster raycaster) => _raycaster = raycaster;

        public void Begin(BlueprintRuntime blueprint)
        {
            if (_preview != null) Destroy(_preview);
            
            var prefab = blueprint.Building.prefab;

            if (prefab == null)
            {
                var nullInst = Instantiate(nullBuildingView, Vector3.zero, Quaternion.identity);
                nullInst.Set(blueprint);
                _preview = nullInst.gameObject;
            }
            else
            {
                _preview = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            }
            
            _transform = _preview.transform;
            _renderers = _preview.GetComponentsInChildren<Renderer>();

            foreach (var r in _renderers) r.enabled = false;

            _rules = blueprint.PlacementRules;
            _lastHit = false;
            
            enabled = true;
        }

        public void End()
        {
            enabled = false;
            
            Destroy(_preview);
            _transform = null;
            _renderers = null;
            _rules = null;
            _lastHit = false;
        }

        private void Update()
        {
            var hit = _raycaster.TryGetGridHit(mask, out var gridHit) && gridHit.IsTop;

            if (hit) _transform.position = GridTransform.CellToWorld(gridHit.Cell);

            if (hit == _lastHit) return;

            foreach (var r in _renderers) r.enabled = hit;

            _lastHit = hit;
        }
    }
}
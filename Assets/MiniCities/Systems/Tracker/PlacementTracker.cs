using System.Collections.Generic;
using MiniCities.Core.Grid;
using MiniCities.Definitions.Block;
using MiniCities.Definitions.Blueprint.Placement;
using MiniCities.Runtime.Blueprint;
using MiniCities.Systems.Map;
using MiniCities.View;
using UnityEngine;

namespace MiniCities.Systems.Tracker
{
    public class PlacementTracker : MonoBehaviour
    {
        [SerializeField] private NullBuildingView nullBuildingView;
        [SerializeField] private LayerMask mask;

        private MapManager _map;
        
        private GridRaycaster _raycaster;
        private BlockView _preview;
        private Transform _transform;
        private Renderer[] _renderers;
        private List<IPlacementRule> _rules;

        private Vector3Int _lastCell;
        private bool _canPlace;

        public void Initialize(GridRaycaster raycaster, MapManager map)
        {
            _raycaster = raycaster;
            _map = map;
        }

        public void Begin(BlueprintRuntime blueprint)
        {
            if (_preview != null) Destroy(_preview.gameObject);
            
            var view = blueprint.Building.view;

            if (view == null)
            {
                var nullInst = Instantiate(nullBuildingView, Vector3.zero, Quaternion.identity);
                nullInst.Set(blueprint);
                _preview = nullInst;
            }
            else
            {
                _preview = Instantiate(view, Vector3.zero, Quaternion.identity);
            }
            
            _transform = _preview.transform;
            _renderers = _preview.GetComponentsInChildren<Renderer>();

            foreach (var r in _renderers) r.enabled = false;

            _rules = blueprint.PlacementRules;
            
            enabled = true;
            
            Debug.Log("Placement Tracking Enabled");
        }

        public void End()
        {
            enabled = false;
            
            Destroy(_preview);
            _transform = null;
            _renderers = null;
            _rules = null;
            _lastCell = default;
            _canPlace = false;
        }

        private void Update()
        {
            if (_raycaster.TryGetGridHit(mask, out var gridHit) && gridHit.IsTop)
            {
                if (gridHit.Cell == _lastCell) return;

                foreach (var r in _renderers) r.enabled = true;

                _lastCell = gridHit.Cell;
                var ctx = new PlacementContext(gridHit, _map);
                _canPlace = true;
                Debug.Log($"Evaluating {_rules.Count} rules at {gridHit.Cell}");

                foreach (var r in _rules)
                {
                    var passed = r.Evaluate(ctx);
                    
                    Debug.Log($"{r.Describe()} -> {passed}");

                    if (passed) continue;
                    
                    _canPlace = false;
                    break;
                }
                
                _preview.SetShader(true, _canPlace);
                _transform.position = GridTransform.CellToWorld(gridHit.Cell);

                return;
            }

            _lastCell = default;

            foreach (var r in _renderers) r.enabled = false;
        }
    }
}
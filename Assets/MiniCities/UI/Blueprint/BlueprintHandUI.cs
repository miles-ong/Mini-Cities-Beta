using System.Collections.Generic;
using DG.Tweening;
using MiniCities.Core.Event;
using MiniCities.Runtime.Blueprint;
using UnityEngine;
using UnityEngine.Splines;

namespace MiniCities.UI.Blueprint
{
    public class BlueprintHandUI : MonoBehaviour
    {
        [Header("Defaults")]
        [SerializeField] private SplineContainer curve;
        [SerializeField] private RectTransform drawPoint;
        [SerializeField] private RectTransform discardPoint;
        [SerializeField] private BlueprintUI blueprintPrefab;

        // add proper game settings soon
        [Header("Draw Settings")]
        [SerializeField] private float updateAnimationSpeed;
        [SerializeField] private float drawSpeed;
        [SerializeField] private float discardSpeed;

        [Header("Interaction Settings")]
        [SerializeField] private float hoverHeight;
        [SerializeField] private float hoverScale;
        [SerializeField] private float hoverAnimationSpeed;
        [SerializeField] private float selectHeight;
        [SerializeField] private float selectAnimationSpeed;

        private Spline _spline;
        private Vector3 _originalScale;

        private readonly List<BlueprintUI> _blueprints = new();
        
        private BlueprintUI _hovered;
        private BlueprintUI _selected;

        private void Start()
        {
            _spline = curve.Spline;
            _originalScale = blueprintPrefab.transform.localScale;
        }

        public void Add(BlueprintRuntime blueprint)
        {
            var ui = Instantiate(blueprintPrefab, drawPoint.position, Quaternion.identity, transform);

            ui.Set(blueprint);
            
            ui.Hovered += OnBlueprintHovered;
            ui.Unhovered += OnBlueprintUnhovered;
            ui.Clicked += OnBlueprintClicked;

            _blueprints.Add(ui);
            UpdateBlueprintPositions();
        }

        public void Remove(BlueprintRuntime blueprint)
        {
            var ui = _blueprints.Find(b => b.Blueprint == blueprint);

            if (ui is null) return;

            _blueprints.Remove(ui);
            Destroy(ui.gameObject);
            UpdateBlueprintPositions();
        }

        private void OnBlueprintHovered(BlueprintUI blueprint)
        {
            _hovered = blueprint;
            UpdateBlueprintPositions();
        }

        private void OnBlueprintUnhovered(BlueprintUI blueprint)
        {
            if (_hovered == blueprint) _hovered = null;
            
            UpdateBlueprintPositions();
        }

        private void OnBlueprintClicked(BlueprintUI blueprint)
        {
            _selected = blueprint;
            EventBus<BlueprintSelectedEvent>.Raise(new BlueprintSelectedEvent(blueprint.Blueprint));
            UpdateBlueprintPositions();
        }

        private void UpdateBlueprintPositions()
        {
            for (var i = 0; i < _blueprints.Count; i++)
            {
                var bp = _blueprints[i];

                var pos = GetCurvePosition(i);
                var rot = GetCurveRotation(i);
                var scale = _originalScale;

                if (bp == _hovered)
                {
                    pos += Vector3.up * hoverHeight;
                    scale *= hoverScale;
                }
                
                if (bp == _selected) pos += Vector3.up * selectHeight;

                var trans = bp.gameObject.transform;
                
                trans.DOLocalMove(pos, updateAnimationSpeed);
                trans.DOLocalRotateQuaternion(rot, updateAnimationSpeed);
                trans.DOScale(scale, updateAnimationSpeed);
            }
        }

        private Vector3 GetCurvePosition(int index) => _spline.EvaluatePosition(GetSplineT(index));

        private Quaternion GetCurveRotation(int index)
        {
            var t = GetSplineT(index);
            var tan = _spline.EvaluateTangent(t);
            var up = _spline.EvaluateUpVector(t);

            return Quaternion.LookRotation(up, Vector3.Cross(up, tan).normalized);
        }

        private float GetSplineT(int index)
        {
            if (_blueprints.Count == 1) return 0.5f;

            var spacing = 1f / _blueprints.Count;
            var firstPos = 0.5f - (_blueprints.Count - 1) * spacing / 2f;

            return firstPos + index * spacing;
        }
    }
}
using System;
using System.Collections.Generic;
using DG.Tweening;
using MiniCities.Core.Event;
using MiniCities.Definitions.Blueprint;
using MiniCities.Runtime.Blueprint;
using UnityEngine;
using UnityEngine.UIElements;

namespace MiniCities.UI.Controllers.Hand
{
    public class BlueprintHandUI
    {
        private readonly VisualElement _container;
        private readonly BlueprintHandSettings _settings;
        private readonly Dictionary<BlueprintEra, VisualTreeAsset> _eraTemplates;
        
        private readonly List<BlueprintElement> _blueprints = new();
        private BlueprintElement _hovered;
        private BlueprintElement _selected;
        private bool _handLowered;

        public BlueprintHandUI(
            VisualElement container,
            BlueprintHandSettings settings,
            VisualTreeAsset modernBlueprintTemplate)
        {
            _container = container;
            _settings = settings;

            _eraTemplates = new Dictionary<BlueprintEra, VisualTreeAsset>()
            {
                { BlueprintEra.Modern, modernBlueprintTemplate }
            };
        }

        public void Add(BlueprintRuntime blueprint)
        {
            var element = new BlueprintElement();

            if (!_eraTemplates.TryGetValue(blueprint.Era, out var template))
                throw new InvalidOperationException($"'{blueprint.Era}' template does not exist!");

            element.BlueprintTemplate = template;
            element.Set(blueprint);
            element.style.scale = new StyleScale(new Vector2(_settings.initialScale, _settings.initialScale));

            element.Hovered += OnBlueprintHovered;
            element.Unhovered += OnBlueprintUnhovered;
            element.Clicked += OnBlueprintClicked;

            _blueprints.Add(element);
            _container.Add(element);

            UpdateBlueprintPositions();
        }

        public void Remove(BlueprintRuntime blueprint)
        {
            var element = _blueprints.Find(x => x.Blueprint == blueprint);

            if (element == null) return;

            element.Hovered -= OnBlueprintHovered;
            element.Unhovered -= OnBlueprintUnhovered;
            element.Clicked -= OnBlueprintClicked;

            if (_hovered == element) _hovered = null;
            if (_selected == element) _selected = null;

            _blueprints.Remove(element);
            element.RemoveFromHierarchy();
            
            UpdateBlueprintPositions();
        }

        public void SetHandLowered(bool lowered)
        {
            if (_handLowered == lowered) return;

            _handLowered = lowered;
            var targetY = lowered ? _settings.loweredHeight : 0f;
            var currentY = _container.resolvedStyle.translate.y;

            DOTween.To(
                () => currentY,
                value =>
                {
                    _container.style.translate = new Translate(new Length(0), new Length(value));
                },
                targetY,
                _settings.lowerAnimationSpeed);
        }

        private void OnBlueprintHovered(BlueprintElement blueprint)
        {
            _hovered = blueprint;
            UpdateBlueprintPositions();
        }

        private void OnBlueprintUnhovered(BlueprintElement blueprint)
        {
            if (_hovered == blueprint) _hovered = null;
            
            UpdateBlueprintPositions();
        }

        private void OnBlueprintClicked(BlueprintElement blueprint)
        {
            if (_selected != blueprint) return;
            
            _selected = blueprint;

            UpdateBlueprintPositions();
            
            GameEventBus<BlueprintSelectedEvent>.Fire(new BlueprintSelectedEvent(blueprint.Blueprint));
        }

        private void UpdateBlueprintPositions()
        {
            for (var i = 0; i < _blueprints.Count; i++)
            {
                var blueprint = _blueprints[i];
                var position = GetCardPosition(i);
                var rotation = GetCardRotation(i);
                var scale = _settings.initialScale;

                if (blueprint == _hovered)
                {
                    position.y -= _settings.selectHeight;
                    scale = _settings.initialScale;
                }

                if (blueprint == _selected) position.y = _settings.selectHeight;
                
                Debug.Log(position);
                AnimateCard(blueprint, position, rotation, scale);
            }
        }

        private Vector2 GetCardPosition(int index)
        {
            if (_blueprints.Count == 1) return Vector2.zero;

            var center = (_blueprints.Count - 1) / 2f;
            var normalized = (index - center) / center;
            var x = (index - center) * _settings.cardSpacing;
            var y = normalized * normalized * _settings.curveHeight;

            return new Vector2(x, y);
        }
        
        private float GetCardRotation(int index)
        {
            if (_blueprints.Count == 1) return 0f;

            var center = (_blueprints.Count - 1) / 2f;
            var normalized = (index - center) / center;

            return normalized * 15f;
        }

        private void AnimateCard(
            BlueprintElement blueprint,
            Vector2 position,
            float rotation,
            float scale)
        {
            var currentX = blueprint.resolvedStyle.translate.x;
            var currentY = blueprint.resolvedStyle.translate.y;
            var targetX = position.x;
            var targetY = position.y;

            DOTween.To(
                () => currentX,
                value =>
                {
                    blueprint.style.translate = new Translate(new Length(value), new Length(currentY));
                },
                targetX,
                _settings.updateAnimationSpeed);
            
            DOTween.To(
                () => currentY,
                value =>
                {
                    blueprint.style.translate = new Translate(new Length(targetX), new Length(value));
                },
                targetY,
                _settings.updateAnimationSpeed);

            blueprint.style.rotate = new Rotate(Angle.Degrees(rotation));
            blueprint.style.scale = new Scale(new Vector2(scale, scale));
        }
    }
}
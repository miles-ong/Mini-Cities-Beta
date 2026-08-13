using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace MiniCities.View.Popup
{
    public class PopupViewStack : MonoBehaviour
    {
        [SerializeField] private Transform anchor;
        [SerializeField] private float verticalSpacing;
        [SerializeField] private float restackDuration;

        private readonly List<PopupView> _activePopups = new();

        private Transform _cameraTransform;

        private void Awake() => _cameraTransform = Camera.main?.transform;
        private void LateUpdate() => transform.forward = _cameraTransform.forward;

        public T Push<T>(T prefab, Action<T> configure) where T : PopupView
        {
            var instance = Instantiate(prefab, anchor);
            instance.transform.parent = transform;
            instance.transform.localPosition = Vector3.zero;
            instance.OnFinished += HandleFinished;
            
            _activePopups.Add(instance);
            configure(instance);
            Restack();

            return instance;
        }

        private void HandleFinished(PopupView popup)
        {
            popup.OnFinished -= HandleFinished;
            _activePopups.Remove(popup);
            Destroy(popup.gameObject);
            Restack();
        }

        private void Restack()
        {
            for (var i = 0; i < _activePopups.Count; i++)
                _activePopups[i].transform.DOLocalMoveY(i * verticalSpacing, restackDuration);
        }
    }
}
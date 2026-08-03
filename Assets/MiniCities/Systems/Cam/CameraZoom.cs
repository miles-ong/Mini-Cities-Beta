using UnityEngine;
using UnityEngine.InputSystem;

namespace MiniCities.Systems.Cam
{
    public class CameraZoom
    {
        private Transform _camera;

        private float _sensitivity;
        private float _minDistance;
        private float _maxDistance;
        
        private float _distance;
        private float _targetDistance;

        public CameraZoom(Transform camera, CameraSettings settings)
        {
            _camera = camera;
            _sensitivity = settings.zoomSensitivity;
            _minDistance = -settings.minDistance;
            _maxDistance = -settings.maxDistance;
            _distance = -settings.startingDistance;
            _targetDistance = _distance;

            UpdatePosition();
        }

        public void Update()
        {
            var scroll = Mouse.current.scroll.ReadValue().y;

            if (scroll != 0)
            {
                _targetDistance += scroll * _sensitivity;
                _targetDistance = Mathf.Clamp(_targetDistance, _maxDistance, _minDistance);
            }

            _distance = Mathf.Lerp(_distance, _targetDistance, Time.deltaTime * 10f);
            
            UpdatePosition();
        }

        private void UpdatePosition() => _camera.localPosition = new Vector3(0, 0, _distance);
    }
}
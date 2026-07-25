using UnityEngine;
using UnityEngine.InputSystem;

namespace _.Systems.Camera
{
    public class CameraRotator
    {
        private Transform _anchor;

        private float _sensitivity;
        private float _minPitch;
        private float _maxPitch;
        
        private float _yaw;
        private float _pitch;

        public CameraRotator(Transform anchor, CameraSettings settings)
        {
            _anchor = anchor;
            _sensitivity = settings.rotationSensitivity;
            _minPitch = settings.minPitch;
            _maxPitch = settings.maxPitch;
            _yaw = settings.startingRotation.x;
            _pitch = settings.startingRotation.y;

            UpdateRotation();
        }

        public void Update()
        {
            if (!Mouse.current.rightButton.isPressed) return;
            
            var delta = Mouse.current.delta.ReadValue();
            _yaw += delta.x * _sensitivity;
            _pitch -= delta.y * _sensitivity;
            _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);

            UpdateRotation();
        }

        private void UpdateRotation() => _anchor.rotation = Quaternion.Euler(_pitch, _yaw, 0);
    }
}
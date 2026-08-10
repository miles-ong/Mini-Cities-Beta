using MiniCities.Core.Event;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MiniCities.Systems.Cam
{
    public class CameraRotator
    {
        private readonly Transform _anchor;

        private float _sensitivity;
        private float _minPitch;
        private float _maxPitch;
        private float _yaw;
        private float _pitch;

        private bool _rotating;

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
            var isPressed = Mouse.current.rightButton.isPressed;

            if (isPressed != _rotating)
            {
                _rotating = isPressed;
                
                if (_rotating) EventBus<CameraRotationStartedEvent>.Fire(new CameraRotationStartedEvent());
                else EventBus<CameraRotationEndedEvent>.Fire(new CameraRotationEndedEvent());
            }

            if (!_rotating) return;
            
            var delta = Mouse.current.delta.ReadValue();
            _yaw += delta.x * _sensitivity;
            _pitch -= delta.y * _sensitivity;
            _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);

            UpdateRotation();
        }

        private void UpdateRotation() => _anchor.rotation = Quaternion.Euler(_pitch, _yaw, 0);
    }
}
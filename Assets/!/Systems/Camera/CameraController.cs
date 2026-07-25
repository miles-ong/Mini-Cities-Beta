using UnityEngine;

namespace _.Systems.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform cameraAnchor;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private CameraSettings settings;

        private CameraRotator _rotator;
        private CameraZoom _zoom;

        private void Awake()
        {
            _rotator = new CameraRotator(cameraAnchor, settings);
            _zoom = new CameraZoom(cameraTransform, settings);
        }

        private void Update()
        {
            _rotator.Update();
            _zoom.Update();
        }

        public void SetAnchor(Vector3 pos) => cameraAnchor.position = pos;
    }
}
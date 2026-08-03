using UnityEngine;

namespace MiniCities.Systems.Cam
{
    [CreateAssetMenu(fileName = "NewCameraSettings", menuName = "Mini Cities/Settings/Camera")]
    public class CameraSettings : ScriptableObject
    {
        [Header("Rotation")] 
        public Vector2 startingRotation;
        public float rotationSensitivity;
        public float minPitch;
        public float maxPitch;

        [Header("Zoom")] 
        public float startingDistance;
        public float zoomSensitivity;
        public float minDistance;
        public float maxDistance;
    }
}

using UnityEngine;

namespace MiniCities.UI.Controllers.Hand
{
    [CreateAssetMenu(fileName = "BPHS_", menuName = "Mini Cities/Settings/Blueprint Hand")]
    public class BlueprintHandSettings : ScriptableObject
    {
        public float initialScale = 0.5f;
        public float cardSpacing = 30f;
        public float curveHeight = 80f;
        public float updateAnimationSpeed = 0.2f;
        public float hoverHeight = 30f;
        public float hoverScale = 0.6f;
        public float selectHeight = 45f;
        public float loweredHeight = 70f;
        public float lowerAnimationSpeed = 0.2f;
    }
}
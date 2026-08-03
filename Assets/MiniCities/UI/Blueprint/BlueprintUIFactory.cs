using UnityEngine;

namespace MiniCities.UI.Blueprint
{
    public class BlueprintUIFactory : MonoBehaviour
    {
        [Header("Backgrounds")]
        [SerializeField] private Sprite residentialBackground;
        [SerializeField] private Sprite commercialBackground;
        [SerializeField] private Sprite industrialBackground;
        [SerializeField] private Sprite civicBackground;
        [SerializeField] private Sprite initiativeBackground;
        [SerializeField] private Sprite disasterBackground;
    }
}
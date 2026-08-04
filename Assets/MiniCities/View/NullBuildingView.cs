using MiniCities.Definitions.Blueprint;
using MiniCities.Runtime.Blueprint;
using TMPro;
using UnityEngine;

namespace MiniCities.View
{
    public class NullBuildingView : MonoBehaviour
    {
        [SerializeField] private TMP_Text displayName;
        [SerializeField] private Renderer renderer;

        private MaterialPropertyBlock _block;
        
        private void Awake() => _block = new MaterialPropertyBlock();

        public void Set(BlueprintRuntime blueprint)
        {
            displayName.text = blueprint.DisplayName;
            renderer.GetPropertyBlock(_block, 1);
            _block.SetColor("_BaseColor", blueprint.Category.ToColor());
            renderer.SetPropertyBlock(_block, 1);
            
            Debug.Log("Set");
        }
    }
}
using MiniCities.Definitions.Blueprint;
using MiniCities.Runtime.Blueprint;
using TMPro;
using UnityEngine;

namespace MiniCities.View
{
    public class NullBuildingView : BuildingView
    {
        [SerializeField] private TMP_Text displayName;
        
        public void Set(BlueprintRuntime blueprint)
        {
            displayName.text = blueprint.DisplayName;
            
            renderer.GetPropertyBlock(Block, 1);
            Block.SetColor("_BaseColor", blueprint.Category.ToColor());
            renderer.SetPropertyBlock(Block, 1);
        }
    }
}
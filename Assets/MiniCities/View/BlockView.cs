using TMPro;
using UnityEngine;

namespace MiniCities.View
{
    public class BlockView : MonoBehaviour
    {
        [SerializeField] protected Renderer renderer;

        protected MaterialPropertyBlock Block;

        private void Awake() => Block = new MaterialPropertyBlock();

        public void SetShader(bool preview, bool canPlace)
        {
            for (var i = 0; i < renderer.sharedMaterials.Length; i++)
            {
                renderer.GetPropertyBlock(Block, i);
            
                Block.SetFloat("_Preview", preview ? 1f : 0f);
                Block.SetFloat("_CanPlace", canPlace ? 1f : 0f);
            
                renderer.SetPropertyBlock(Block, i);
            }
        }
    }
}
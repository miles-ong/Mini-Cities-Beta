using DG.Tweening;
using TMPro;
using UnityEngine;

namespace MiniCities.View.Popup
{
    public class TextPopupView : PopupView
    {
        [Header("Unique References")] 
        [SerializeField] private TextMeshPro label;

        public void Show(string text, Color backgroundColor)
        {
            label.text = text;
            label.ForceMeshUpdate();
            Play(backgroundColor);
        }

        protected override void ResizeBackground()
        {
            label.ForceMeshUpdate();
            
            var size = label.GetPreferredValues();

            background.size = new Vector2(size.x + padding.x * 2f, size.y + padding.y * 2f);
        }
        
        protected override void SetContentAlpha(float alpha) => label.alpha = alpha;
        protected override Tween FadeContent(float target, float duration) => label.DOFade(target, duration);
    }
}
using System;
using DG.Tweening;
using UnityEngine;

namespace MiniCities.View.Popup
{
    public abstract class PopupView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform container;
        [SerializeField] protected SpriteRenderer background;
        
        [Header("Animation")]
        [SerializeField] private float riseDistance;
        [SerializeField] private float lifetime;
        [SerializeField] private float fadeInDuration;
        [SerializeField] private float fadeOutDuration;

        [Header("Padding")] 
        [SerializeField] protected Vector2 padding;
        
        private Sequence _sequence;

        public event Action<PopupView> OnFinished;
        
        protected void OnDestroy() => _sequence?.Kill();

        protected void Play(Color backgroundColor)
        {
            ResizeBackground();
            SetContentAlpha(0f);
            background.color = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, 0f);
            container.localScale = Vector3.one * 0.8f;
            container.localPosition = Vector3.zero;

            _sequence?.Kill();

            _sequence = DOTween.Sequence()
                .Append(FadeContent(1f, fadeInDuration))
                .Join(background.DOFade(0.9f, fadeInDuration))
                .Join(container.DOScale(1f, fadeInDuration).SetEase(Ease.OutBack))
                .Join(container.DOLocalMoveY(riseDistance, lifetime + fadeInDuration))
                .AppendInterval(Mathf.Max(0f, lifetime - fadeInDuration))
                .Append(FadeContent(0f, fadeOutDuration))
                .Join(background.DOFade(0f, fadeOutDuration))
                .OnComplete(() => OnFinished?.Invoke(this));
        }

        protected abstract void ResizeBackground();
        protected abstract void SetContentAlpha(float alpha);
        protected abstract Tween FadeContent(float target, float duration);
    }
}
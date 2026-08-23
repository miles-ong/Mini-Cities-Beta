using DG.Tweening;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

[UxmlElement]
public partial class CultureMeter : VisualElement
{
    [UxmlAttribute] public int BarCount { get => _bars.Length; set => SetBars(value); }

    [UxmlAttribute]
    public Color BarColor
    {
        get => _barColor;
        set
        {
            _barColor = value;
            ApplyBarColor();
        }
    }
    [UxmlAttribute] public float BarSpacing { get; set; } = 4f;
    [UxmlAttribute] public float MinHeightPercent { get; set; } = 2f;
    [UxmlAttribute] public float BounceDuration { get; set; } = 0.45f;
    [UxmlAttribute] public float StaggerDelay { get; set; } = 0.06f;
    [UxmlAttribute] public float IdleJitterPercent { get; set; } = 3f;
    [UxmlAttribute] public float IdleJitterSpeed { get; set; } = 2.2f;
    [CreateProperty]
    [UxmlAttribute]
    public float Value
    {
        get => _targetValue;
        set
        {
            if (Mathf.Approximately(_targetValue, value)) return;
            
            SetValue(value);
        }
    }
    
    private VisualElement[] _bars = new VisualElement[3];
    private Color _barColor = Color.white;
    private float[] _barMultiplier;
    private float[] _barPhase;
    private float[] _barBaseHeight01;
    private Tween[] _tweens;
    private float _targetValue;
    private float _elapsed;

    public CultureMeter()
    {
        style.flexDirection = FlexDirection.Row;
        style.alignItems = Align.FlexEnd;
        style.justifyContent = Justify.SpaceBetween;
        
        SetBars(BarCount);
        SetValue(Value);

        schedule.Execute(Tick).Every(16);
    }

    private void SetBars(int count)
    {
        if (_bars != null) 
            foreach (var bar in _bars) 
                if (bar != null) Remove(bar);
        
        _bars = new VisualElement[count];
        _barMultiplier = new float[count];
        _barPhase = new float[count];
        _barBaseHeight01 = new float[count];
        _tweens = new Tween[count];
        
        for (var j = 0; j < count; j++)
        {
            var bar = new VisualElement();
            bar.style.flexGrow = 1;
            bar.style.flexBasis = 0;
            bar.style.marginLeft = j == 0 ? 0 : BarSpacing / 2f;
            bar.style.marginRight = j == count - 1 ? 0 : BarSpacing / 2f;
            bar.style.backgroundColor = _barColor;
            bar.style.borderTopLeftRadius = 2;
            bar.style.borderTopRightRadius = 2;
            bar.style.height = Length.Percent(MinHeightPercent);
            Add(bar);

            _bars[j] = bar;
            _barMultiplier[j] = Random.Range(0.78f, 1.18f);
            _barPhase[j] = Random.Range(0f, Mathf.PI * 2f);
            _barBaseHeight01[j] = 0f;
        }
    }

    private void ApplyBarColor()
    {
        if (_bars == null) return;
        
        foreach (var bar in _bars)
            if (bar != null) bar.style.backgroundColor = _barColor;
    }

    private void SetValue(float v, bool animate = true)
    {
        v = Mathf.Clamp01(v);
        _targetValue = v;

        for (var i = 0; i < BarCount; i++)
        {
            _tweens[i]?.Kill();
            var target = Mathf.Clamp01(v * _barMultiplier[i]);

            if (!animate)
            {
                _barBaseHeight01[i] = target;
                continue;
            }

            var index = i;
            _tweens[i] = DOTween.To(
                    () => _barBaseHeight01[index],
                    x => _barBaseHeight01[index] = x,
                    target, BounceDuration)
                .SetDelay(StaggerDelay * i)
                .SetEase(Ease.OutElastic, 1.1f, 0.6f);
            
            Debug.Log($"Animating: {i}");
        }
    }

    private void Tick(TimerState timerState)
    {
        _elapsed += timerState.deltaTime / 1000f;

        for (var i = 0; i < BarCount; i++)
        {
            var jitter = Mathf.Sin(_elapsed * IdleJitterSpeed + _barPhase[i]) * IdleJitterPercent;
            var heightPercent = Mathf.Clamp(_barBaseHeight01[i] * 100f + jitter, MinHeightPercent, 100f);
            _bars[i].style.height = Length.Percent(heightPercent);
        }
    }
}
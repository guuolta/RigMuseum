using System;
using UniRx;
using UnityEngine.UI;

public class SliderBase : UIPartBase
{
    private Slider _slider;
    private Image _handle;
    private IObservable<float> _sliderValueAsObservable;
    /// <summary>
    /// スライダーの値
    /// </summary>
    public IObservable<float> SliderValueAsObservable => _sliderValueAsObservable;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _handle = _slider.handleRect.GetComponent<Image>();
        SetEventSliderChange();
        SetEventPointer(_handle);
    }

    public float GetMinValue()
    {
        return _slider.minValue;
    }

    public float GetMaxValue()
    {
        return _slider.maxValue;
    }

    private void SetEventSliderChange()
    {
        _sliderValueAsObservable = _slider.OnValueChangedAsObservable();
    }

    public void SetValue(float value)
    {
        _slider.value = value;
    }
}
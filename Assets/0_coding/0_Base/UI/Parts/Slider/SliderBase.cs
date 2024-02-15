using System;
using UniRx;
using UnityEngine.UI;

/// <summary>
/// スライダーのベース
/// </summary>
public class SliderBase : UIPartBase
{
    private Slider _slider;
    /// <summary>
    /// スライダー
    /// </summary>
    public Slider Slider
    {
        get
        {
            if(_slider == null)
            {
                _slider = GetComponent<Slider>();
            }

            return _slider;
        }
    }
    private Image _handle;
    /// <summary>
    /// スライダーのハンドル
    /// </summary>
    public Image Handle
    {
        get
        {
            if(_handle == null)
            {
                _handle = GetComponent<Image>();
            }

            return _handle;
        }
    }
    private IObservable<float> _sliderValueAsObservable;
    /// <summary>
    /// スライダーの値
    /// </summary>
    public IObservable<float> SliderValueAsObservable
    {
        get
        {
            if( _sliderValueAsObservable == null)
            {
                _sliderValueAsObservable = Slider.OnValueChangedAsObservable();
            }

            return _sliderValueAsObservable;
        }
    }

    public override void SetEvent()
    {
        SetEventPointer(_handle);
    }

    public void SetSlider(float minValue, float maxValue)
    {
        Slider.minValue = minValue;
        Slider.maxValue = maxValue;
    }

    public void SetValue(float value)
    {
        Slider.value = value;
    }
}
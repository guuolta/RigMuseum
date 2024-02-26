using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToggleBase : UIPartBase
{
    [Header("トグルON時の画像")]
    [SerializeField]
    private Image _onImage;
    [Header("トグルOFF時の画像")]
    [SerializeField]
    private Image _offImage;

    private BoolReactiveProperty _isToggle = new BoolReactiveProperty(false);
    /// <summary>
    /// トグルのOn、Off
    /// </summary>
    public BoolReactiveProperty IsToggle => _isToggle;
    private Slider _slider;
    protected Slider slider
    {
        get
        {
            if (_slider == null)
            {
                _slider.GetComponent<Slider>();
                _slider.interactable = false;
                _slider.minValue = 0;
                _slider.maxValue = 1;
                _slider.wholeNumbers = true;
                _slider.value = 0;
            }
            return _slider;
        }
    }
    private HandleBase _handle;
    protected HandleBase handle
    {
        get
        {
            if( _handle == null)
            {
                slider.handleRect.GetComponent<HandleBase>();

                if(_handle == null)
                {
                    _handle = slider.handleRect.AddComponent<HandleBase>();
                }
            }

            return _handle;
        }
    }

    protected override void SetEvent()
    {
        SetEventClick();
    }

    private void SetEventClick()
    {
        handle.OnClickCallback += () =>
        {
            _isToggle.Value = !_isToggle.Value;
        };
    }

    public void SetEventCheckBox()
    {
        IsToggle
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(async value =>
            {
                if(value)
                {
                    await slider.DOValue(1, animationTime)
                        .SetEase(Ease.InSine)
                        .AsyncWaitForCompletion();

                }
                else
                {
                    slider.value = 0;
                }
            });
    }

    public void SetValue(bool value)
    {
        _isToggle.Value = value;
    }
}

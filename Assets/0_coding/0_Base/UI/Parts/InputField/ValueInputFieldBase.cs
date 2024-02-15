using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ValueInputFieldBase : UIPartBase
{
    private float _minValue;
    private float _maxValue;
    private TMP_InputField _inputField;
    /// <summary>
    /// インプットフィールド
    /// </summary>
    public TMP_InputField InputField
    { 
        get 
        { 
            if(_inputField == null)
            {
                _inputField = GetComponent<TMP_InputField>();
            }
            return _inputField; 
        } 
    }
    private Image _image;
    /// <summary>
    /// インプットフィールドの画像
    /// </summary>
    public Image Image
    {
        get
        {
            if(_image == null)
            {
                _image = GetComponent<Image>();
            }

            return _image;
        }
    }
    private ReactiveProperty<float> _inputValueAsObservable = new ReactiveProperty<float>();
    /// <summary>
    /// InputFieldの値
    /// </summary>
    public ReactiveProperty<float> InputValueAsObservable => _inputValueAsObservable;
    public UnityAction<string> OnChangeEvent;

    public override void SetFirstEvent()
    {
        SetEventPointer(Image);
        SetEventInputValue();
    }

    /// <summary>
    /// インプットフィールドの初期値設定
    /// </summary>
    /// <param name="minValue"> インプットフィールドの最小値 </param>
    /// <param name="maxValue"> インプットフィールドの最大値 </param>
    public void SetInputField(float minValue, float maxValue)
    {
        _minValue = minValue;
        _maxValue = maxValue;
    }
    
    /// <summary>
    /// インプットフィールドの値を設定
    /// </summary>
    private void SetEventInputValue()
    {
        OnChangeEvent = (value) =>
        {
            float volume;
            if (float.TryParse(value, out volume))
            {
                InputValueAsObservable.Value = Mathf.Clamp(volume, _minValue, _maxValue);
            }
            else
            {
                InputValueAsObservable.Value = 0;
            }
        };

        InputField.onValueChanged.AddListener(OnChangeEvent);
    }


    /// <summary>
    /// インプットフィールドの値を設定
    /// </summary>
    /// <param name="value"> 値 </param>
    public void SetValue(float value)
    {
        InputField.text = value.ToString();
    }
}

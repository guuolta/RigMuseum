using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ValueInputField : UIPartBase
{
    private InputField _inputField;
    private Image _image;
    private ReactiveProperty<float> _inputValueAsObservable = new ReactiveProperty<float>();
    /// <summary>
    /// InputFieldの値
    /// </summary>
    public ReactiveProperty<float> InputValueAsObservable => _inputValueAsObservable;

    private void Awake()
    {
        _inputField = GetComponent<InputField>();
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        SetEventPointer(_image);
    }

    public void SetEventInputValue(float minValue, float maxValue)
    {
        float result = 0;
        _inputField.OnValueChangedAsObservable()
            .Where(value => float.TryParse(value, out result))
            .Subscribe(value =>
            {
                InputValueAsObservable.Value = Mathf.Clamp(result, minValue, maxValue);
            }).AddTo(this);
    }

    public void SetValue(float value)
    {
        _inputField.text = value.ToString();
    }
}

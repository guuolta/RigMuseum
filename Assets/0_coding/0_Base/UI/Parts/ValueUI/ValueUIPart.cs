using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// スライダーとインプットフィールドをつなげる
/// </summary>
[System.Serializable]
public class ValueUIPart : UIBase
{
    [Header("スライダーとインプットフィールドの最小値")]
    [SerializeField]
    private float _minValue;
    [Header("スライダーとインプットフィールドの最大値")]
    [SerializeField]
    private float _maxValue;
    [Header("対象のスライダー")]
    [SerializeField]
    private SliderBase _slider;
    [Header("対象のインプットフィールド")]
    [SerializeField]
    private ValueInputFieldBase _inputField;
    private ReactiveProperty<float> _value = new ReactiveProperty<float>();
    /// <summary>
    /// 値
    /// </summary>
    public ReactiveProperty<float> Value => _value;
    private List<IDisposable> _disposables = new List<IDisposable>();

    public override void SetEvent()
    {
        _slider.SetSlider(_minValue, _maxValue);
        _inputField.SetInputField(_minValue, _maxValue);
        SetEventChangeValue();
    }

    public void OnDestroy()
    {
        foreach (var disposable in _disposables)
        {
            disposable.Dispose();
        }
        _disposables.Clear();
    }

    /// <summary>
    /// スライダーとインプットフィールドの値のイベント
    /// </summary>
    private void SetEventChangeValue()
    {
        _slider.SliderValueAsObservable
            .Skip(1)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                _inputField.SetValue(value);
                _value.Value = value;
            }).AddTo(_disposables);

        _inputField.InputValueAsObservable
            .Skip(1)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                _slider.SetValue(value);
                _value.Value = value;
            }).AddTo(_disposables);
    }

    /// <summary>
    /// 値を設定する
    /// </summary>
    /// <param name="value"> 値 </param>
    public void SetValue(float value)
    {
        _slider.SetValue(value);
        _inputField.SetValue(value);
        _value.Value = value;
    }
}

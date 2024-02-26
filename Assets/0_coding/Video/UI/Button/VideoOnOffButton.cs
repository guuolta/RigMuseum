using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VideoOnOffButton : ButtonBase
{
    [Header("ON時のボタンの画像")]
    [SerializeField]
    private Image _onButtonImage;
    [Header("OFF時のボタンの画像")]
    [SerializeField]
    private Image _offButtonImage;
    [Header("ON時の説明のUI")]
    [SerializeField]
    private VideoExplainText _onExplainText;
    [Header("OFF時の説明のUI")]
    [SerializeField]
    private VideoExplainText _offExplainText;

    private BoolReactiveProperty _isOn = new BoolReactiveProperty(false);
    public BoolReactiveProperty IsOn => _isOn;
    private Image _targetImage;
    private VideoExplainText _targetExplainText;

    protected override void Init()
    {
        base.Init();
        _targetImage = _onButtonImage;
        _targetExplainText = _onExplainText;
        _onButtonImage.color += new Color(0, 0, 0, 1);
        _offButtonImage.color -= new Color(0, 0, 0, 1);
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventClick();
        SetEventIsOn();
    }

    private void SetEventClick()
    {
        OnClickCallback += () =>
        {
            _isOn.Value = !_isOn.Value;
            
        };
    }

    /// <summary>
    /// ボタンがOn、Offの時のイベント
    /// </summary>
    private void SetEventIsOn()
    {
        IsOn
            .Skip(1)
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(async value =>
            {
                HideAsync(_targetImage, Ct).Forget();
                _targetExplainText.HideAsync().Forget();
                _targetImage = value ? _offButtonImage : _onButtonImage;
                _targetExplainText = value ? _offExplainText : _onExplainText;
                ShowAsync(_targetImage, Ct).Forget();
                await _targetExplainText.ShowAsync();
            });
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
    }

    public override async void OnPointerEnter(PointerEventData eventData)
    {
        await _targetExplainText.ShowAsync();
    }

    public override async void OnPointerExit(PointerEventData eventData)
    {
        await _targetExplainText.HideAsync();
    }
}

using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class VideoOnOffButton : ButtonBase
{
    [Header("ON時の説明のUI")]
    [SerializeField]
    private VideoExplainText _onExplainText;
    [Header("OFF時の説明のUI")]
    [SerializeField]
    private VideoExplainText _offExplainText;

    private BoolReactiveProperty _isOn = new BoolReactiveProperty(false);
    public BoolReactiveProperty IsOn => _isOn;
    private VideoExplainText _targetExplainText;

    public override void Init()
    {
        base.Init();
        _targetExplainText = _offExplainText;
    }

    public override void SetEvent()
    {
        base.SetEvent();
        SetEventIsOn();
    }

    public override void SetEventButton()
    {
        base.SetEventButton();
        onClickCallback += () =>
        {
            _isOn.Value = !_isOn.Value;
            
        };
    }

    private void SetEventIsOn()
    {
        IsOn
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(async value =>
            {
                _targetExplainText.HideAsync().Forget();
                _targetExplainText = value ? _onExplainText : _offExplainText;
                await _targetExplainText.ShowAsync();
            });
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        onClickCallback?.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
    }

    public override async void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            return;
        }

        await _targetExplainText.ShowAsync();
    }

    public override async void OnPointerExit(PointerEventData eventData)
    {
        await UniTask.WaitUntil(() => !Input.GetMouseButton(0));

        await _targetExplainText.HideAsync();
    }
}

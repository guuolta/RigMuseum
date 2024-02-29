using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class VideoToggle : ToggleBase
{
    [Header("ON時の説明のUI")]
    [SerializeField]
    private VideoExplainText _onExplainText;
    [Header("OFF時の説明のUI")]
    [SerializeField]
    private VideoExplainText _offExplainText;
    private VideoExplainText _targetExplainText;

    protected override void Init()
    {
        _targetExplainText = _offExplainText;
        handle.SetOffSe();
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventToggle();
    }

    public override async void OnPointerEnter(PointerEventData eventData)
    {
        await _targetExplainText.ShowAsync(Ct);
    }

    public override async void OnPointerExit(PointerEventData eventData)
    {
        await _targetExplainText.HideAsync(Ct);
    }

    private void SetEventToggle()
    {
        handle.OnClickCallback += () =>
        {
            _targetExplainText.HideAsync(Ct).Forget();
            _targetExplainText = IsToggle.Value ? _onExplainText : _offExplainText;
            _targetExplainText.ShowAsync(Ct).Forget();
        };
    }
}
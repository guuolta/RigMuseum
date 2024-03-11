using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class VideoSlider : SliderBase
{
    [Header("説明のテキスト")]
    [SerializeField]
    private ExplainText _explainText;

    protected override void Init()
    {
        SetSlider(0, 10);
    }

    public override async void OnPointerEnter(PointerEventData eventData)
    {
        await _explainText.ShowAsync(Ct);
    }

    public override async void OnPointerExit(PointerEventData eventData)
    {
        await _explainText.HideAsync(Ct);
    }
}
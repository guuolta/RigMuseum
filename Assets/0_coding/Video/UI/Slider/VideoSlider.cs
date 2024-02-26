using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class VideoSlider : SliderBase
{
    [Header("説明のテキスト")]
    [SerializeField]
    private VideoExplainText _explainText;

    protected override void Init()
    {
        SetSlider(0, 10);
    }

    public override async void OnPointerEnter(PointerEventData eventData)
    {
        await _explainText.ShowAsync();
    }

    public override async void OnPointerExit(PointerEventData eventData)
    {
        await _explainText.HideAsync();
    }

    private void SetEventSlider()
    {
        SliderValueAsObservable
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
            });
    }
}
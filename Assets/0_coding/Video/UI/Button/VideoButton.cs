using UnityEngine;
using UnityEngine.EventSystems;

public class VideoButton : ButtonBase
{
    [Header("説明のテキスト")]
    [SerializeField]
    private VideoExplainText _explainText;

    public override void OnPointerUp(PointerEventData eventData)
    {
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
    }

    public override async void OnPointerEnter(PointerEventData eventData)
    {
        await _explainText.ShowAsync();
    }

    public override async void OnPointerExit(PointerEventData eventData)
    {
        await _explainText.HideAsync();
    }
}

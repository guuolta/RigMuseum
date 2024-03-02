using UnityEngine;
using UnityEngine.EventSystems;

public class VideoButton : ButtonBase
{
    [Header("説明のテキスト")]
    [SerializeField]
    private VideoExplainText _explainText;

    protected override void SetEventPlaySe()
    {

    }

    public override void OnPointerUp(PointerEventData eventData)
    {
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
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

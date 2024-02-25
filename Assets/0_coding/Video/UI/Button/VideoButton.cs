using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VideoButton : ButtonBase
{
    [Header("説明のUI")]
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
        if (Input.GetMouseButton(0))
        {
            return;
        }

        await _explainText.ShowAsync();
    }

    public override async void OnPointerExit(PointerEventData eventData)
    {
        await UniTask.WaitUntil(() => !Input.GetMouseButton(0));

        await _explainText.HideAsync();
    }
}

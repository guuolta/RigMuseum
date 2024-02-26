using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnderButton : ButtonBase
{
    /// <summary>
    /// ボタンにカーソルがあった時のイベントときのイベント
    /// </summary>
    public System.Action onEnterEvent;

    [Header("カーソルがあった時に、ボタンが上がる量")]
    [SerializeField]
    private float _upValue;
    [Header("カーソルがあった時のボタンの濃さ")]
    [Range(0f, 1f)]
    [SerializeField]
    private float _alpha = 0.8f;

    private float _iniPosY;
    private float _iniAlpha;

    protected override void Init()
    {
        base.Init();
        _iniPosY = RectTransform.anchoredPosition.y;
        _upValue += _iniPosY;
        _iniAlpha = CanvasGroup.alpha;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            return;
        }

        //onEnterEvent.Invoke();
        CanvasGroup.DOFade(_alpha, AnimationTime).SetEase(Ease.InSine);
    }

    public override async void OnPointerExit(PointerEventData eventData)
    {
        await UniTask.WaitUntil(() => !Input.GetMouseButton(0));
        base.OnPointerExit(eventData);
        RectTransform.DOAnchorPosY(_iniPosY, AnimationTime).SetEase(Ease.OutSine);
        CanvasGroup.DOFade(_iniAlpha, AnimationTime).SetEase(Ease.OutSine);
    }
}

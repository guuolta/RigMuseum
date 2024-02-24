using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPanelView : PanelViewBase
{
    [Header("スライドする前の位置")]
    [SerializeField]
    private float _slideBeforePos;
    [Header("スライドする後の位置")]
    [SerializeField]
    private float _slideAfterPos;

    public override async UniTask ShowAsync(float animeTime)
    {
        await RectTransform.DOAnchorPosX(_slideBeforePos, animeTime)
            .SetEase(Ease.InSine)
            .AsyncWaitForCompletion();
    }

    public override async UniTask HideAsync(float animeTime)
    {
        await RectTransform.DOAnchorPosX(_slideAfterPos, animeTime)
            .SetEase(Ease.OutSine)
            .AsyncWaitForCompletion();
    }
}

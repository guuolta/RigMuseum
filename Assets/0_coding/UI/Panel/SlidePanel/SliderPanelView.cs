using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

public class SliderPanelView : PanelViewBase
{
    [Header("スライドする前の位置")]
    [SerializeField]
    private float _slideBeforePos;
    [Header("スライドする後の位置")]
    [SerializeField]
    private float _slideAfterPos;

    public override async UniTask ShowAsync(float animeTime, CancellationToken ct)
    {
        await RectTransform
            .DOAnchorPosX(_slideBeforePos, animeTime)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken: ct);
    }

    public override async UniTask HideAsync(float animeTime, CancellationToken ct)
    {
        await RectTransform
            .DOAnchorPosX(_slideAfterPos, animeTime)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken: ct);
    }
}

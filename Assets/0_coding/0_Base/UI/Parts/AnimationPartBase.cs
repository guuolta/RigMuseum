using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimationPartBase : UIPartBase
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        Transform
            .DOScale(0.8f, animationTime)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken: Ct)
            .Forget();
        CanvasGroup
            .DOFade(0.8f, animationTime)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken: Ct)
            .Forget();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        Transform.DOScale(1f, animationTime)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken: Ct)
            .Forget();
        CanvasGroup.DOFade(1f, animationTime)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken: Ct)
            .Forget();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            return;
        }

        Transform
            .DOScale(1.2f, animationTime)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken: Ct)
            .Forget();
    }

    public override async void OnPointerExit(PointerEventData eventData)
    {
        await UniTask.WaitUntil(() => !Input.GetMouseButton(0), cancellationToken: Ct);

        Transform
            .DOScale(1f, animationTime)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken: Ct)
            .Forget();
    }
}
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIAnimationPartBase : UIPartBase
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        Transform.DOScale(0.8f, AnimationTime).SetEase(Ease.InSine);
        CanvasGroup.DOFade(0.8f, AnimationTime).SetEase(Ease.InSine);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        Transform.DOScale(1f, AnimationTime).SetEase(Ease.OutSine);
        CanvasGroup.DOFade(1f, AnimationTime).SetEase(Ease.OutSine);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            return;
        }

        Transform.DOScale(1.2f, AnimationTime).SetEase(Ease.InSine);
    }

    public override async void OnPointerExit(PointerEventData eventData)
    {
        await UniTask.WaitUntil(() => !Input.GetMouseButton(0));

        Transform.DOScale(1f, AnimationTime).SetEase(Ease.OutSine);
    }
}

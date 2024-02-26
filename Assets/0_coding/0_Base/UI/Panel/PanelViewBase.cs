using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// パネルのビューのベース
/// </summary>
public class PanelViewBase : ViewBase
{
    protected override void Init()
    {
        Transform.localScale = Vector3.zero;
    }

    public override async UniTask ShowAsync(float animeTime)
    {
        if(Transform.localScale != Vector3.zero)
        {
            return;
        }

        await Transform.DOScale(Vector2.one, animeTime)
            .SetEase(Ease.InSine)
            .AsyncWaitForCompletion();
    }
    
    public override async UniTask HideAsync(float animeTime)
    {
        if(Transform.localScale == Vector3.zero)
        {
            return;
        }

        await Transform.DOScale(Vector2.zero, animeTime)
            .SetEase(Ease.OutSine)
            .AsyncWaitForCompletion();
    }
}

using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
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

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        if(Transform.localScale != Vector3.zero)
        {
            return;
        }

        await Transform
            .DOScale(Vector2.one, animationTime)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken: ct);
    }
    
    public override async UniTask HideAsync(CancellationToken ct)
    {
        if(Transform.localScale == Vector3.zero)
        {
            return;
        }

        await Transform
            .DOScale(Vector2.zero, animationTime)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken: ct);
    }
}

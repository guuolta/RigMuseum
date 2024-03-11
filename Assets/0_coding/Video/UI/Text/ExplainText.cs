using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UniRx;
using UnityEngine;

public class ExplainText : UIPartBase
{
    protected override void Init()
    {
        CanvasGroup.alpha = 0f;
        ChangeInteractive(false);
    }

    /// <summary>
    /// UI表示
    /// </summary>
    /// <returns></returns>
    public async UniTask ShowAsync(CancellationToken ct)
    {
        CanvasGroup.DOComplete();

        await ShowAsync(CanvasGroup, ct);
        ChangeInteractive(true);
    }

    /// <summary>
    /// UI消す
    /// </summary>
    /// <returns></returns>
    public async UniTask HideAsync(CancellationToken ct)
    {
        CanvasGroup.DOComplete();

        await HideAsync(CanvasGroup, ct);
        ChangeInteractive(false);
    }
}

using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Threading;

public class VideoExplainText : UIPartBase
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

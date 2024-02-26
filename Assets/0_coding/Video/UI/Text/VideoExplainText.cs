using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class VideoExplainText : UIPartBase
{
    public override void Init()
    {
        CanvasGroup.alpha = 0f;
        ChangeInteractive(false);
    }

    /// <summary>
    /// UI表示
    /// </summary>
    /// <returns></returns>
    public async UniTask ShowAsync()
    {
        CanvasGroup.DOComplete();

        await CanvasGroup
            .DOFade(1, AnimationTime)
            .SetEase(Ease.InSine)
            .AsyncWaitForCompletion();

        ChangeInteractive(true);
    }

    /// <summary>
    /// UI消す
    /// </summary>
    /// <returns></returns>
    public async UniTask HideAsync()
    {
        CanvasGroup.DOComplete();

        await CanvasGroup
            .DOFade(0, AnimationTime)
            .SetEase(Ease.OutSine)
            .AsyncWaitForCompletion();
        ChangeInteractive(false);
    }
}

using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class ExplainText : UIBase
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
        await ShowAsync(CanvasGroup, ct);
        ChangeInteractive(true);
    }

    /// <summary>
    /// UI消す
    /// </summary>
    /// <returns></returns>
    public async UniTask HideAsync(CancellationToken ct)
    {
        await HideAsync(CanvasGroup, ct);
        ChangeInteractive(false);
    }
}

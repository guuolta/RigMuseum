using Cysharp.Threading.Tasks;
using System.Threading;

public class UIFadeAnimationUIPartBase : UIPartBase
{
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

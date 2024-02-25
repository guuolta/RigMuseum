using Cysharp.Threading.Tasks;

/// <summary>
/// ビューベース
/// </summary>
public abstract class ViewBase : UIBase
{
    /// <summary>
    /// UIを表示
    /// </summary>
    /// <param name="animationTime"> アニメーションの時間 </param>
    /// <returns></returns>
    public abstract UniTask ShowAsync(float animationTime);

    /// <summary>
    /// UIを消す
    /// </summary>
    /// <param name="animationTime"> アニメーションの時間 </param>
    /// <returns></returns>
    public abstract UniTask HideAsync(float animationTime);
}
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

/// <summary>
/// プレゼンターベース
/// </summary>
/// <typeparam name="TView"> ビュー </typeparam>
public class PresenterBase<TView> : ObjectBase, IPresenter
    where TView : ViewBase
{
    [Header("パネルのアニメーションの時間")]
    [SerializeField]
    protected float animationTime = 0.1f;
    private TView _view;
    /// <summary>
    /// ビュー
    /// </summary>
    protected TView View
    {
        get
        {
            if (_view == null)
            {
                _view = GetComponent<TView>();
            }

            return _view;
        }
    }

    public virtual async UniTask ShowAsync(CancellationToken ct)
    {
        await View.ShowAsync(animationTime, ct);
    }

    public virtual async UniTask HideAsync(CancellationToken ct)
    {
        await View.HideAsync(animationTime, ct);
    }
}

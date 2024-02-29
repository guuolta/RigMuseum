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
        await View.ShowAsync(ct);
        View.ChangeInteractive(true);
    }

    public virtual async UniTask HideAsync(CancellationToken ct)
    {
        View.ChangeInteractive(false);
        await View.HideAsync(ct);
    }
}

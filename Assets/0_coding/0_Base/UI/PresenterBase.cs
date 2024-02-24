using Cysharp.Threading.Tasks;
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
    private float _animationTime = 0.1f;
    private TView _view;
    /// <summary>
    /// ビュー
    /// </summary>
    public TView View
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

    public virtual async UniTask ShowAsync()
    {
        await View.ShowAsync(_animationTime);
    }

    public virtual async UniTask HideAsync()
    {
        await View.HideAsync(_animationTime);
    }
}

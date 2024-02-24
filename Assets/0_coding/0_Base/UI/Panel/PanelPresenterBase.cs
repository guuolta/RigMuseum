using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// パネルのプレゼンター
/// </summary>
/// <typeparam name="TView"> パネルビュー </typeparam>
public class PanelPresenterBase<TView> : UIBase, IPanelPresenter
    where TView : PanelViewBase
{
    [Header("パネルのアニメーションの時間")]
    [SerializeField]
    private float _animationTime = 0.1f;
    private TView _view;
    public TView View
    {
        get
        {
            if(_view == null)
            {
                _view = GetComponent<TView>();
            }

            return _view;
        }
    }

    /// <summary>
    /// パネルを開く
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask OpenPanelAsync()
    {
        await View.ShowPanelAsync(_animationTime);
    }

    /// <summary>
    /// パネルを閉じる
    /// </summary>
    /// <returns></returns>
    public virtual async UniTask ClosePanelAsync()
    {
        await View.ClosePanelAsync(_animationTime);
    }
}

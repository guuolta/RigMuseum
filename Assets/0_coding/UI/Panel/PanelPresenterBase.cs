using Cysharp.Threading.Tasks;
using UnityEngine;

public class PanelPresenterBase : MonoBehaviour
{
    [Header("パネルのアニメーションの時間")]
    [SerializeField]
    private float _animationTime;
    public PanelViewBase View;

    public virtual async UniTask OpenPanelAsync()
    {
        await View.ShowPanelAsync(_animationTime);
    }

    public virtual async UniTask ClosePanelAsync()
    {
        await View.ClosePanelAsync(_animationTime);
    }
}

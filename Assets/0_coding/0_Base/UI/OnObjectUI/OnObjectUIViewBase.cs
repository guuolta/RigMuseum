using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class OnObjectUIViewBase : ViewBase
{
    [Header("閉じるボタン")]
    [SerializeField]
    private ButtonBase _closeButton;
    public ButtonBase CloseButton => _closeButton;

    protected override void Init()
    {
        Hide(CanvasGroup);
    }

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        await ShowAsync(CanvasGroup, ct);
    }

    public override async UniTask HideAsync(CancellationToken ct)
    {
        await HideAsync(CanvasGroup, ct);
    }
}

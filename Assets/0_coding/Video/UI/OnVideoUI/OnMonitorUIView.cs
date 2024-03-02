using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class OnMonitorUIView : ViewBase
{
    [Header("閉じるボタン")]
    [SerializeField]
    private CloseButton _closeButton;
    public CloseButton CloseButton => _closeButton;

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

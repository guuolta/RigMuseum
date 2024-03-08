using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class ObjectNamePanelView : ViewBase
{
    private TMP_Text _nameText;
    public TMP_Text NameText
    {
        get
        {
            if (_nameText == null)
            {
                _nameText = GetComponentInChildren<TMP_Text>();
            }
            return _nameText;
        }
    }

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

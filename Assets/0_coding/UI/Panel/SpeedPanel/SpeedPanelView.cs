using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpeedPanelView : ViewBase
{
    private SpeedPanelCell[] _speedCells;
    public SpeedPanelCell[] SpeedCells => _speedCells;

    protected override void Init()
    {
        SetSpeedCellList();
        ChangeInteractive(false);
    }

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        await ShowAsync(CanvasGroup, ct);
    }

    public override async UniTask HideAsync(CancellationToken ct)
    {
        await HideAsync(CanvasGroup, ct);
    }

    private void SetSpeedCellList()
    {
        List<SpeedPanelCell> speedCellList = new List<SpeedPanelCell>();

        for (int i = 0; i < Transform.childCount; i++)
        {
            var cell = Transform.GetChild(i).GetComponent<SpeedPanelCell>();
            if (cell == null)
            {
                continue;
            }

            speedCellList.Add(cell);
        }

        _speedCells = speedCellList.ToArray();
    }
}

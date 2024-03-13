using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MusicPlayerPresenter : PanelPresenterBase<MusicPlayerPanelView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventVolumeButton(Ct);
    }

    private void SetEventVolumeButton(CancellationToken ct)
    {
        View.VolumeButton.OnClickCallback += async () =>
        {
            await View.VolumePanel.ShowAsync(ct);
        };
    }
}

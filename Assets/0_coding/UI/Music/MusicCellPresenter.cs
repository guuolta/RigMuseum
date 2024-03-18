using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

public class MusicCellPresenter : MusicCellPresenterBase<MusicCellView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventPlayButton(Ct);
    }

    /// <summary>
    /// 再生ボタンのイベント設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventPlayButton(CancellationToken ct)
    {
        PhonographMusicPlayerManager.Instance.ID
            .TakeUntilDestroy(this)
            .Select(id => id == ID)
            .DistinctUntilChanged()
            .Subscribe(async value =>
            {
                if(value)
                {
                    await View.ShowAsync(ct);
                }
                else
                {
                    await View.HideAsync(ct);
                }
            });

        PhonographMusicPlayerManager.Instance.IsPlay
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                View.MusicPlayButton.SetOn(!value);
            });

        View.MusicPlayButton.IsOn
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                if (!value)
                {
                    PhonographMusicPlayerManager.Instance.Play();
                }
                else
                {
                    PhonographMusicPlayerManager.Instance.Pause();
                }
            });
    }
}

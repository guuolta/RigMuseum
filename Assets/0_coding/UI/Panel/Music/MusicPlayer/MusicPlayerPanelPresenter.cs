using System.Threading;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;

public class MusicPlayerPanelPresenter : PanelPresenterBase<MusicPlayerPanelView>
{
    private ReactiveProperty<int> _playTime => PhonographMusicPlayerManager.Instance.PlayTime;
    private ReactiveProperty<int> _length => PhonographMusicPlayerManager.Instance.Length;
    private CompositeDisposable disposables = new CompositeDisposable();

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventPlayButton();
        SetEventNextButton();
        SetEventBackButton();
        SetEventLoopButton();
        SetEventShuffleButton();
        SetEventVolumeButton(Ct);
        SetPlayListButton(Ct);
        SyncSeekBarAndTime();
        SetEventTimeText();
    }

    /// <summary>
    /// 再生ボタンのイベント設定
    /// </summary>
    private void SetEventPlayButton()
    {
        PhonographMusicPlayerManager.Instance.IsPlay
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                SetPlayButton(value);
            });

        View.PlayButton.IsOn
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                if(!value)
                {
                    PhonographMusicPlayerManager.Instance.Play();
                }
                else
                {
                    PhonographMusicPlayerManager.Instance.Pause();
                }
            });
    }

    /// <summary>
    /// 次の曲を再生するボタン
    /// </summary>
    private void SetEventNextButton()
    {
        View.NextButton.OnClickCallback += () =>
        {
            PhonographMusicPlayerManager.Instance.PlayNext();
        };
    }

    /// <summary>
    /// 前の曲を再生するボタン
    /// </summary>
    private void SetEventBackButton()
    {
        View.BackButton.OnClickCallback += () =>
        {
            PhonographMusicPlayerManager.Instance.PlayBack();
        };
    }

    /// <summary>
    /// ループボタンのイベント設定
    /// </summary>
    private void SetEventLoopButton()
    {
        View.LoopButton.ActiveIndex
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Select(value => (LoopType)(value))
            .Subscribe(value =>
            {
                switch(value)
                {
                    case LoopType.Off:
                        PhonographMusicPlayerManager.Instance.SetLoop(false);
                        PhonographMusicPlayerManager.Instance.SetPlayback(false);
                        break;
                    case LoopType.PlayBack:
                        PhonographMusicPlayerManager.Instance.SetLoop(false);
                        PhonographMusicPlayerManager.Instance.SetPlayback(true);
                        break;
                    case LoopType.On:
                        PhonographMusicPlayerManager.Instance.SetLoop(true);
                        PhonographMusicPlayerManager.Instance.SetPlayback(false);
                        break;
                }
            });
    }

    /// <summary>
    /// ループボタンの状態
    /// </summary>
    private enum LoopType
    {
        Off,
        PlayBack,
        On
    }

    /// <summary>
    /// シャッフルボタンのイベント設定
    /// </summary>
    private void SetEventShuffleButton()
    {
        View.ShuffleButton.ActiveIndex
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Select(value => (ShuffleType)(value))
            .Subscribe(value =>
            {
                switch(value)
                {
                    case ShuffleType.Off:
                        Debug.Log("シャッフルオフ");
                        break;
                    case ShuffleType.On:
                        Debug.Log("シャッフルオン");
                        break;
                    case ShuffleType.Favorite:
                        Debug.Log("お気に入り");
                        break;
                }
            });
    }

    /// <summary>
    /// シャッフルボタンの状態
    /// </summary>
    private enum ShuffleType
    {
        Off,
        On,
        Favorite
    }

    /// <summary>
    /// 音量ボタンのイベント設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventVolumeButton(CancellationToken ct)
    {
        View.VolumeButton.OnClickCallback += async () =>
        {
            await PhonographMusicPlayerManager.Instance.ShowOverlayPanelAsync(View.VolumePanel, ct);
        };

        View.VolumePanel.IsMute
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                View.VolumeButton.SetOn(value);
            });
    }

    /// <summary>
    /// プレイリストボタンのイベント設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetPlayListButton(CancellationToken ct)
    {
        View.PlaylistButton.IsOn
            .Skip(1)
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(async value =>
            {
                if(value)
                {
                    await View.ShowPlayListAsync(ct);
                }
                else
                {
                    await View.HidePlayListAsync(ct);
                }
            });
    }

    /// <summary>
    /// シークバーと動画の時間の同期
    /// </summary>
    private void SyncSeekBarAndTime()
    {
        View.SeekBar.OnPointerUpEvent += () =>
        {
            disposables = DisposeEvent(disposables);
            Play();
        };
        View.SeekBar.OnPointerDownEvent += () =>
        {
            Pause();
            SetEventSeekBar();
        };

        _playTime
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                View.SeekBar.SetValue(value);
            });

        _length
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                View.SeekBar.SetSlider(0, value);
            });
    }

    /// <summary>
    /// シークバーのイベント設定
    /// </summary>
    private void SetEventSeekBar()
    {
        View.SeekBar.SliderValueAsObservable
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                PhonographMusicPlayerManager.Instance.SetTime(value);
            }).AddTo(disposables);
    }

    /// <summary>
    /// 再生時間のテキストのイベント設定
    /// </summary>
    private void SetEventTimeText()
    {
        _playTime
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                View.PlayTimeText.text = PhonographMusicPlayerManager.Instance.GetMusicTime(value);
                View.RemainTimeText.text = PhonographMusicPlayerManager.Instance.GetMusicTime(_length.Value - value);
            });
    }

    /// <summary>
    /// 再生
    /// </summary>
    public void Play()
    {
        SetPlayButton(true);
    }

    /// <summary>
    /// 停止
    /// </summary>
    public void Pause()
    {
        SetPlayButton(false);
    }

    /// <summary>
    /// 再生ボタンの設定
    /// </summary>
    /// <param name="isPlay"> 再生するか </param>
    public void SetPlayButton(bool isPlay)
    {
        View.PlayButton.SetOn(!isPlay);
    }
}
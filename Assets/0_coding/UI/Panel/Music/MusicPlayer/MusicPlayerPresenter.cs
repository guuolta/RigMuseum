using System.Threading;
using UnityEngine;
using UniRx;
using System;
using static UnityEngine.Rendering.DebugUI;

public class MusicPlayerPresenter : PanelPresenterBase<MusicPlayerPanelView>
{
    private ReactiveProperty<int> _playTime => AudioManager.Instance.BGMPlayTime;
    private ReactiveProperty<int> _length => AudioManager.Instance.BGMLength;
    private CompositeDisposable disposables = new CompositeDisposable();

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventMask(Ct);
        SetEventPlayButton();
        SetEventNextButton();
        SetEventBackButton();
        SetEventLoopButton();
        SetEventShuffleButton();
        SetEventVolumeButton(Ct);
        SyncSeekBarAndTime();
        SetEventTimeText();
    }

    /// <summary>
    /// マスク画像のイベント設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventMask(CancellationToken ct)
    {
        View.MaskImage.OnClickCallback += async () =>
        {
            if(View.VolumePanel.IsOpen.Value)
                await View.VolumePanel.HideAsync(ct);
        };

        View.VolumePanel.IsOpen
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                View.MaskImage.ChangeInteractive(value);
            });
    }

    /// <summary>
    /// 再生ボタンのイベント設定
    /// </summary>
    private void SetEventPlayButton()
    {
        View.PlayButton.IsOn
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                if(!value)
                {
                    AudioManager.Instance.PlayBGM();
                }
                else
                {
                    AudioManager.Instance.PauseBGM();
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
            Debug.Log("next");
        };
    }

    /// <summary>
    /// 前の曲を再生するボタン
    /// </summary>
    private void SetEventBackButton()
    {
        View.BackButton.OnClickCallback += () =>
        {
            Debug.Log("back");
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
                        AudioManager.Instance.SetBGMLoop(false);
                        break;
                    case LoopType.Auto:
                        AudioManager.Instance.SetBGMLoop(false);
                        Debug.Log("オート");
                        break;
                    case LoopType.On:
                        AudioManager.Instance.SetBGMLoop(true);
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
        Auto,
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
            await View.VolumePanel.ShowAsync(ct);
        };
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
                AudioManager.Instance.SetBGMTime(value);
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
                View.PlayTimeText.text = GetVideoTime(value);
                View.RemainTimeText.text = GetVideoTime(_length.Value - value);
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

    /// <summary>
    /// 再生時間のテキストを取得
    /// </summary>
    /// <param name="time"> 再生時間 </param>
    /// <returns></returns>
    private string GetVideoTime(int time)
    {
        if (time < 0)
        {
            time = 0;
        }

        int minutes = time / 60;
        int seconds = time % 60;

        return $"{minutes:00}:{seconds:00}";
    }
}
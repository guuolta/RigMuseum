using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;

public class VideoUIPresenter : PresenterBase<VideoUIView>
{
    private CompositeDisposable disposables = new CompositeDisposable();

    protected override void Init()
    {
        View.ChangeInteractive(false);
    }

    protected override void SetEvent()
    {
        SetEventScreen(Ct);
        SyncSeekBarAndTime(Ct);
        SetEventSeekBar();
        SetEventPlayButton(Ct);
        SetEventSpeedButton(Ct);
        SetEventMuteButton();
        SetEventVolumeSlider();
        SetEventVideoTime();
        SetEventToggle();
    }


    protected override void Destroy()
    {
        DisposeEvent(disposables);
    }

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        SetInitVolumeSliderValue();
        await base.ShowAsync(ct);
    }

    /// <summary>
    /// スクリーン画像のイベント設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventScreen(CancellationToken ct)
    {
        View.ScreenImage.OnClickCallback += () =>
        {
            if(GameVideoManager.Instance.IsPlayVideo.Value)
            {
                Pause(ct);
            }
            else
            {
                Play(ct);
            }
        };
    }

    /// <summary>
    /// シークバーと動画の時間の同期
    /// </summary>
    /// <param name="ct"></param>
    private void SyncSeekBarAndTime(CancellationToken ct)
    {
        View.SeekBar.OnPointerUpEvent += () =>
        {
            disposables = DisposeEvent(disposables);
            Play(ct);
        };
        View.SeekBar.OnPointerDownEvent += () =>
        {
            Pause(ct);
            SetEventSeekBar();
        };

        GameVideoManager.Instance.VideoTime
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                View.SeekBar.SetSlider(0, value);
            });

        GameVideoManager.Instance.VideoPlayTime
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                View.SeekBar.SetValue(value);
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
                GameVideoManager.Instance.SetVideoTime(value);
            }).AddTo(disposables);
    }

    /// <summary>
    /// 再生ボタンの設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventPlayButton(CancellationToken ct)
    {
        View.PlayButton.IsOn
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                if(value)
                {
                    GameVideoManager.Instance.Pause(ct).Forget();
                }
                else
                {
                    GameVideoManager.Instance.Play(ct).Forget();
                }
            });
    }

    private void SetEventBackButton()
    {
        View.BackButton.OnClickCallback += () =>
        {

        };
    }

    private void SetEventSkipButton()
    {
        View.SkipButton.OnClickCallback += () =>
        {

        };
    }

    /// <summary>
    /// 再生速度ボタンの設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventSpeedButton(CancellationToken ct)
    {
        View.SpeedButton.OnClickCallback += async () =>
        {
            if(View.SpeedPanel.IsOpen)
            {
                await View.SpeedPanel.HideAsync(ct);
            }
            else
            {
                await View.SpeedPanel.ShowAsync(ct);
            }
        };

        View.SpeedPanel.OnIndex
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {

            });
    }

    /// <summary>
    /// ミュートボタンの設定
    /// </summary>
    private void SetEventMuteButton()
    {
        View.MuteButton.IsOn
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                GameVideoManager.Instance.SetMute(value);    
            });
    }

    /// <summary>
    /// 音量スライダーの初期設定
    /// </summary>
    private void SetInitVolumeSliderValue()
    {
        View.VolumeSlider.SetValue(AudioManager.Instance.GetSoundVolume(AudioType.Movie));
    }

    /// <summary>
    /// 音量スライダーの設定
    /// </summary>
    private void SetEventVolumeSlider()
    {
        View.VolumeSlider.SliderValueAsObservable
            .Skip(1)
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                AudioManager.Instance.SetMovieVolume(value);
            });
            
    }

    /// <summary>
    /// 再生時間の設定
    /// </summary>
    private void SetEventVideoTime()
    {
        GameVideoManager.Instance.VideoTime
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                View.VideoTimeText.text = GetVideoTime(value);
            });

        GameVideoManager.Instance.VideoPlayTime
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                View.PlayTimeText.text = GetVideoTime(value);
            });
    }

    /// <summary>
    /// 自動再生トグルの設定
    /// </summary>
    private void SetEventToggle()
    {
        View.Toggle.IsToggle
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                GameVideoManager.Instance.SetLoop(value);
            });

    }

    /// <summary>
    /// 動画再生
    /// </summary>
    /// <param name="ct"></param>
    private void Play(CancellationToken ct)
    {
        View.PlayButton.SetOn(false);
        GameVideoManager.Instance.Play(ct).Forget();
    }

    /// <summary>
    /// 動画停止
    /// </summary>
    /// <param name="ct"></param>
    private void Pause(CancellationToken ct)
    {
        View.PlayButton.SetOn(true);
        GameVideoManager.Instance.Pause(ct).Forget();
    }

    /// <summary>
    /// 再生時間のテキストを取得
    /// </summary>
    /// <param name="time"> 再生時間 </param>
    /// <returns></returns>
    private string GetVideoTime(int time)
    {
        if(time < 0)
        {
            time = 0;
        }

        int minutes = time / 60;
        int seconds = time % 60;

        return $"{minutes:00}:{seconds:00}";
    }
}

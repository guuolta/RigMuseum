using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;

public class VideoUIPresenter : PresenterBase<VideoUIView>
{
    [Header("動画を飛ばす時間")]
    [SerializeField]
    private float _skipTime =  10f;
    [Header("動画を戻す時間")]
    [SerializeField]
    private float _backTime = 10f;

    private bool _isShow = true;
    public bool IsShow => _isShow;

    private CompositeDisposable disposables = new CompositeDisposable();

    protected override void Init()
    {
        View.ChangeInteractive(false);
    }

    protected override void SetEvent()
    {
        SetEventScreen(Ct);
        SyncSeekBarAndTime(Ct);
        SetEventPlayButton(Ct);
        SetEventSkipButton();
        SetEventBackButton();
        SetEventNextButton(Ct);
        SetEventSpeedButton(Ct);
        SetEventMuteButton();
        SetEventVolumeSlider();
        SetEventVideoTime();
        SetEventToggle(Ct);
    }

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        _isShow = false;
        SetInitVolumeSliderValue();
        await base.ShowAsync(ct);
        _isShow = true;
    }

    /// <summary>
    /// スクリーン画像のイベント設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventScreen(CancellationToken ct)
    {
        View.ScreenImage.OnClickCallback += () =>
        {
            if(View.SpeedPanel.IsOpen.Value)
            {
                View.SpeedPanel.HideAsync(ct).Forget();
                return;
            }

            if(GameVideoManager.Instance.IsPlayVideo.Value)
            {
                Pause();
            }
            else
            {
                Play();
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
            Play();
        };
        View.SeekBar.OnPointerDownEvent += () =>
        {
            Pause();
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
            .Subscribe(async value =>
            {
                if(!value)
                {
                    await GameVideoManager.Instance.PlayAsync(ct);
                }
                else
                {
                    await GameVideoManager.Instance.PauseAsync(ct);
                }
            });
    }

    /// <summary>
    /// 動画再生
    /// </summary>
    private void Play()
    {
        View.PlayButton.SetOn(false);
    }

    /// <summary>
    /// 動画停止
    /// </summary>
    private void Pause()
    {
        View.PlayButton.SetOn(true);
    }

    /// <summary>
    /// 再生ボタンに設定
    /// </summary>
    /// <param name="isPlay"> 再生中か </param>
    public void SetPlayButton(bool isPlay)
    {
        View.PlayButton.SetOn(!isPlay);
    }

    /// <summary>
    /// 時間飛ばしボタンのイベント設定
    /// </summary>
    private void SetEventSkipButton()
    {
        View.SkipButton.OnClickCallback += () =>
        {
            GameVideoManager.Instance.Skip(_skipTime);
        };
    }

    /// <summary>
    /// 時間戻しボタンのイベント設定
    /// </summary>
    private void SetEventBackButton()
    {
        View.BackButton.OnClickCallback += () =>
        {
            GameVideoManager.Instance.Back(_backTime);
        };
    }

    /// <summary>
    /// 次の動画再生ボタンのイベント設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventNextButton(CancellationToken ct)
    {
        View.NextButton.OnClickCallback += async () =>
        {
            await GameVideoManager.Instance.PlayNextAsync(ct);
        };
    }

    /// <summary>
    /// 再生速度ボタンのイベント設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventSpeedButton(CancellationToken ct)
    {
        View.SpeedButton.OnClickCallback += async () =>
        {
            if(View.SpeedPanel.IsOpen.Value)
            {
                await View.SpeedPanel.HideAsync(ct);
            }
            else
            {
                await View.SpeedPanel.ShowAsync(ct);
            }
        };


        View.SpeedPanel.OnIndex
            .Skip(1)
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                GameVideoManager.Instance.SetVideoSpeed(GetVideoSpeed(value));
            });
    }

    /// <summary>
    /// ミュートボタンのイベント設定
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
        View.VolumeSlider.SetValue(AudioManager.Instance.GetSoundVolume(AudioType.Video));
    }

    /// <summary>
    /// 音量スライダーのイベント設定
    /// </summary>
    private void SetEventVolumeSlider()
    {
        View.VolumeSlider.SliderValueAsObservable
            .Skip(1)
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                AudioManager.Instance.SetVolume(AudioType.Video, value);
            });
            
    }

    /// <summary>
    /// 再生時間のイベント設定
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
    /// 自動再生トグルのイベント設定
    /// </summary>
    private void SetEventToggle(CancellationToken ct)
    {
        View.Toggle.IsToggle
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                GameVideoManager.Instance.SetLoop(!value);
                GameVideoManager.Instance.SetAutoPlayNext(value, ct);
            });

    }

    

    /// <summary>
    /// 再生速度取得
    /// </summary>
    /// <param name="index"> 速度のセル </param>
    /// <returns></returns>
    private float GetVideoSpeed(int index)
    {
        return index * 0.25f;
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

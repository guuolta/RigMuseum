using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.Video;

public class YoutubeVideoPlayer : ObjectBase
{
    private VideoPlayer _videoPlayer;
    public VideoPlayer VideoPlayer
    {
        get
        {
            if(_videoPlayer == null)
            {
                _videoPlayer = GetComponent<VideoPlayer>();
                _videoPlayer.playOnAwake = false;
            }

            return _videoPlayer;
        }
    }
    private YoutubePlayer.YoutubePlayer _youtubePlayer;
    public YoutubePlayer.YoutubePlayer YoutubePlayer
    {
        get
        {
            if(_youtubePlayer == null)
            {
                _youtubePlayer = GetComponent<YoutubePlayer.YoutubePlayer>();
            }

            return _youtubePlayer;
        }
    }
    private AudioSource _audioSource;
    public AudioSource AudioSource
    {
        get
        {
            if( _audioSource == null)
            {
                _audioSource = VideoPlayer.GetTargetAudioSource(0);
            }

            return _audioSource;
        }
    }

    private BoolReactiveProperty _isPlayVideo = new BoolReactiveProperty(false);
    /// <summary>
    /// 動画が再生されているか
    /// </summary>
    public BoolReactiveProperty IsPlayVideo => _isPlayVideo;
    private BoolReactiveProperty _isSetVideo = new BoolReactiveProperty(false);
    /// <summary>
    /// 動画が設定されたか
    /// </summary>
    public BoolReactiveProperty IsSetVideo => _isSetVideo;
    private ReactiveProperty<int> _videoPlayTime = new ReactiveProperty<int>(0);
    /// <summary>
    /// 動画の再生時間
    /// </summary>
    public ReactiveProperty<int> VideoPlayTime => _videoPlayTime;
    private ReactiveProperty<int> _videoTime = new ReactiveProperty<int>(0);
    /// <summary>
    /// 動画の時間
    /// </summary>
    public ReactiveProperty<int> VideoTime => _videoTime;

    private CompositeDisposable disposables = new CompositeDisposable();

    protected override void SetEvent()
    {
        SetEventVideoPlay();
    }

    protected override void Destroy()
    {
        DisposeEvent(disposables);
    }

    protected override CompositeDisposable DisposeEvent(CompositeDisposable disposable)
    {
        _videoPlayTime.Value = 0;
        return base.DisposeEvent(disposable);
    }

    /// <samarry>
    /// 設定されている動画再生
    /// </samarry>
    public void Play()
    {
        if(AudioSource.mute)
        {
            AudioSource.mute = false;
        }

        if(!_isPlayVideo.Value)
        {
            VideoPlayer.Play();
        }
    }

    /// <summary>
    /// 動画再生
    /// </summary>
    /// <param name="youtubeURL"> 再生する動画のURL </param>
    /// <returns></returns>
    public async UniTask Play(string youtubeURL, CancellationToken ct)
    {
        _isSetVideo.Value = false;
        await YoutubePlayer.PlayVideoAsync(youtubeURL, cancellationToken:ct);
        _isSetVideo.Value = true;
        SetEventPlayTime();
        _videoTime.Value = GetVideoTime();
    }

    /// <summary>
    /// 動画を止める
    /// </summary>
    public void Pause()
    {
        if (_isPlayVideo.Value)
        {
            VideoPlayer.Pause();
        }
    }

    public void SetVideoPlayTime(float time)
    {
        VideoPlayer.time = time;
        VideoPlayTime.Value = (int)time;
    }

    /// <summary>
    /// ビデオの時間を取得
    /// </summary>
    /// <returns></returns>
    private int GetVideoTime()
    {
        return (int)YoutubePlayer.VideoPlayer.length;
    }

    /// <summary>
    /// 動画が再生したときのイベント設定
    /// </summary>
    private void SetEventVideoPlay()
    {
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Select(_ => VideoPlayer.isPlaying)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                _isPlayVideo.Value = value;
            });
    }

    /// <summary>
    /// 動画が終了したときのイベント設定
    /// </summary>
    private void SetEventVideoFinish()
    {
        VideoPlayer.loopPointReached += SetEventLoopPointReached;
    }

    /// <summary>
    /// 動画が終了したときの処理
    /// </summary>
    /// <param name="vp"></param>
    private void SetEventLoopPointReached(VideoPlayer vp)
    {
        
    }

    /// <summary>
    /// 動画時間を設定
    /// </summary>
    private void SetEventPlayTime()
    {
        disposables = DisposeEvent(disposables);

        Observable.EveryUpdate()
            .Select(value => VideoPlayer.time)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                VideoPlayTime.Value = (int)value;
            }).AddTo(disposables);
    }


    /// <summary>
    /// ループするか設定
    /// </summary>
    /// <param name="isLoop"> ループするか </param>
    public void SetLoop(bool isLoop)
    {
        _videoPlayer.isLooping = isLoop;
    }
}

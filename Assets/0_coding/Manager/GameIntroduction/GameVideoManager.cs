using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameVideoManager : SingletonObjectBase<GameVideoManager>
{
    [Header("ゲームデータ")]
    [SerializeField]
    private GameDatas gameDatas;
    [Header("モニター")]
    [SerializeField]
    private Monitor _monitor;
    [Header("ビデオプレイヤー")]
    [SerializeField]
    private YoutubeVideoPlayer _videoPlayer;
    [Header("動画のUIキャンバス")]
    [SerializeField]
    private Canvas _videoCanvas;
    [Header("動画UIプレゼンター")]
    [SerializeField]
    private VideoUIPresenter _videoUIPresenter;
    [Header("モニターの範囲検知用画像")]
    [SerializeField]
    private Image _rangeImage;
    [Header("アニメーションの時間")]
    [SerializeField]
    private float _animationTime = 0.1f;
    [Header("モニターにターゲット時のモニターとプレイヤーの距離")]
    [SerializeField]
    private float _targetDistance = 10f;
    [Header("モニターにターゲット解除時のモニターとプレイヤーの距離")]
    [SerializeField]
    private float _targetClearDistance = 15f;

    /// <summary>
    /// 動画が再生されているか
    /// </summary>
    public BoolReactiveProperty IsPlayVideo => _videoPlayer.IsPlayVideo;
    /// <summary>
    /// 動画の再生時間
    /// </summary>
    public ReactiveProperty<int> VideoPlayTime => _videoPlayer.VideoPlayTime;
    /// <summary>
    /// 動画の時間
    /// </summary>
    public ReactiveProperty<int> VideoTime => _videoPlayer.VideoTime;

    private BoolReactiveProperty _isEnterUI = new BoolReactiveProperty(false);
    private Vector3 _targetPos = Vector3.zero;
    private Vector3 _targetRot = Vector3.zero;
    private Vector3 _clearPos = Vector3.zero;
    private Camera _camera;
    private RectTransform _canvasRectTransform;
    private CompositeDisposable _disposables = new CompositeDisposable();
    private Vector2 MousePos = Vector2.zero;

    protected override void Init()
    {
        _videoPlayer.Play(gameDatas.GetGameYoutubeURL(0), Ct).Forget();

        _targetPos = _monitor.Transform.position + (_monitor.Transform.right * _targetDistance);
        _targetRot = _monitor.Transform.localEulerAngles + new Vector3(0, -90, 0);
        _clearPos = _targetPos + (_monitor.Transform.right * _targetClearDistance);
        
        _camera = _videoCanvas.worldCamera;
        _canvasRectTransform = _videoCanvas.GetComponent<RectTransform>();
        _rangeImage.color -= new Color(0, 0, 0, 1);
        _rangeImage.enabled = false;
    }

    protected override void SetEvent()
    {
        SetEventTarget();
        SetEventVideo(Ct);
    }

    protected override void Destroy()
    {
        DisposeEvent(_disposables);
    }

    /// <summary>
    /// 動画再生
    /// </summary>
    public async UniTask Play(CancellationToken ct)
    {
        await UniTask.WaitUntil(() => _videoPlayer.IsSetVideo.Value, cancellationToken: ct);
        _videoPlayer.Play();
    }

    /// <summary>
    /// 動画停止
    /// </summary>
    public async UniTask Pause(CancellationToken ct)
    {
        await UniTask.WaitUntil(() => _videoPlayer.IsSetVideo.Value, cancellationToken: ct);
        _videoPlayer.Pause();
    }

    /// <summary>
    /// 再生時間まで動画を飛ばす
    /// </summary>
    /// <param name="time"> 時間 </param>
    public void SetVideoTime(float time)
    {
        _videoPlayer.SetVideoPlayTime(time);
    }

    /// <summary>
    /// 再生速度設定
    /// </summary>
    /// <param name="speed"> 再生速度 </param>
    public void SetVideoSpeed(float speed)
    {
        _videoPlayer.VideoPlayer.playbackSpeed = speed;
    }

    /// <summary>
    /// 動画をミュートするか設定
    /// </summary>
    /// <param name="mute"> ミュートにするか </param>
    public void SetMute(bool mute)
    {
        _videoPlayer.AudioSource.mute = mute;
    }


    /// <summary>
    /// 動画をループするか設定
    /// </summary>
    /// <param name="isLoop"> ループするか </param>
    public void SetLoop(bool isLoop)
    {
        _videoPlayer.SetLoop(isLoop);
    }

    /// <summary>
    /// モニターにターゲット時のイベント設定
    /// </summary>
    private void SetEventTarget()
    {
        GameStateManager.MuseumStatus
            .TakeUntilDestroy(this)
            .Select(value => value == MuseumState.Monitor)
            .SkipWhile(value => !value)
            .DistinctUntilChanged()
            .Subscribe(async value =>
            {
                if(value)
                {
                    await PlayerManager.Instance.TargetObjectAsync(_animationTime, _targetPos, _targetRot, Ct);
                    SetEventPointer();
                    _rangeImage.enabled = true;
                }
                else
                {
                    DisposePointerEvent();
                    _rangeImage.enabled = false;
                    await PlayerManager.Instance.ClearTargetAsync(_animationTime, _clearPos, Ct);
                }
            });
    }

    /// <summary>
    /// ポインターのイベント設定
    /// </summary>
    private void SetEventPointer()
    {
        Observable.EveryUpdate()
            .Select(_ => EventSystem.current.IsPointerOverGameObject())
            .Subscribe(value =>
            {
                _isEnterUI.SetValueAndForceNotify(value);
            }).AddTo(_disposables);

        _isEnterUI
            .Subscribe(_ =>
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, Input.mousePosition, _camera, out MousePos);

                if(_canvasRectTransform.rect.Contains(MousePos))
                {
                    _videoUIPresenter.ShowAsync(Ct).Forget();
                }
                else
                {
                    _videoUIPresenter.HideAsync(Ct).Forget();
                    AudioManager.Instance.SaveVolume();
                }
            }).AddTo(_disposables);
    }

    /// <summary>
    /// 各ステートにおける動画の設定
    /// </summary>
    private void SetEventVideo(CancellationToken ct)
    {
        GameStateManager.MuseumStatus
            .Skip(1)
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(async value =>
            {
                switch (value)
                {
                    case MuseumState.Play:
                        _videoPlayer.AudioSource.spatialBlend = 1;
                        await Play(ct);
                        break;
                    case MuseumState.Monitor:
                        _videoPlayer.AudioSource.spatialBlend = 0;
                        await Play(ct);
                        break;
                    case MuseumState.Pause:
                        await Pause(ct);
                        break;
                    default:
                        SetMute(true);
                        break;
                }
            });
    }

    private void DisposePointerEvent()
    {
        if(_disposables == new CompositeDisposable())
        {
            return;
        }

        _disposables = DisposeEvent(_disposables);
    }
}

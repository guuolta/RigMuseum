using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;

public class GameVideoManager : SingletonObjectBase<GameVideoManager>
{
    [Header("ゲームデータ")]
    [SerializeField]
    private GameDatas _gameDatas;
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
    [Header("キャプションUI")]
    [SerializeField]
    private GameCaptionPresenter _captionUI;
    [Header("モニターの範囲検知用画像")]
    [SerializeField]
    private Image _rangeImage;
    [Header("ローディングUI")]
    [SerializeField]
    private VideoLoadingPresenter _loadingUI;
    [Header("モニター上のUI")]
    [SerializeField]
    private OnMonitorUIPresenter _onMonitorUI;
    [Header("アニメーションの時間")]
    [SerializeField]
    private float _animationTime = 0.1f;
    [Header("モニターにターゲット時のモニターとプレイヤーの距離")]
    [SerializeField]
    private float _targetDistance = 10f;
    [Header("モニターにターゲット解除時のモニターとプレイヤーの距離")]
    [SerializeField]
    private float _targetClearDistance = 15f;


    private ReactiveProperty<int> _videoIndex = new ReactiveProperty<int>(0);
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

    private GameData _targetGameData = new GameData();
    private Vector3 _targetPos = Vector3.zero;
    private Vector3 _targetRot = Vector3.zero;
    private Vector3 _clearPos = Vector3.zero;
    private Camera _camera;
    private RectTransform _canvasRectTransform;
    private CompositeDisposable _videoDisposables = new CompositeDisposable();
    private CompositeDisposable _pointerDisposables = new CompositeDisposable();

    protected override void Init()
    {
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
        SetEventVideoPlay(Ct);
    }

    /// <summary>
    /// 動画再生
    /// </summary>
    public async UniTask PlayAsync(CancellationToken ct)
    {
        await UniTask.WaitUntil(() => _videoPlayer.IsSetVideo.Value, cancellationToken: ct);
        _videoPlayer.Play();
    }

    /// <summary>
    /// 次の動画を再生
    /// </summary>
    public void PlayNext()
    {
        if(_videoIndex.Value == _gameDatas.GetCount() - 1)
        {
            _videoIndex.Value = 0;
            return;
        }

        _videoIndex.Value++;
    }

    /// <summary>
    /// 動画停止
    /// </summary>
    public void Pause()
    {
        if(!_videoPlayer.IsSetVideo.Value)
        {
            return;
        }

        _videoPlayer.Pause();
    }

    /// <summary>
    /// 時間飛ばし
    /// </summary>
    /// <param name="time"> 飛ばす時間 </param>
    public void Skip(float time)
    {
        SetVideoTime(VideoPlayTime.Value + time);
    }

    /// <summary>
    /// 時間戻し
    /// </summary>
    /// <param name="time"> 戻す時間 </param>
    public void Back(float time)
    {
        SetVideoTime(VideoPlayTime.Value - time);
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
    /// 動画の再生イベントの設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventVideoPlay(CancellationToken ct)
    {
        _videoIndex
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(async value =>
            {
                _targetGameData = _gameDatas.GetGameData(value);

                _loadingUI.ShowAsync(ct).Forget();
                Pause();
                await _videoPlayer.Play(_targetGameData.YoutubeURL, ct);
                SetGameDataToUI(_targetGameData);
                _loadingUI.HideAsync(ct).Forget();
            });
    }

    /// <summary>
    /// ゲームデータをキャプションに設定
    /// </summary>
    /// <param name="gameData"></param>
    private void SetGameDataToUI(GameData gameData)
    {
        _captionUI.SetTitleText(gameData.Title);
        _captionUI.SetCoadingMemberText(gameData.CoadingMenber);
        _captionUI.SetIllustrationMemberText(gameData.IllustrationMenber);
        _captionUI.SetModelMemberText(gameData.ModelMenber);
        _captionUI.SetDTMMemberText(gameData.DTMMenber);
        _captionUI.SetURL(gameData.GameURL);
        _captionUI.SetExplain(gameData.Explain);
    }

    /// <summary>
    /// 動画の自動再生設定
    /// </summary>
    /// <param name="isAuto"> 自動再生するか </param>
    /// <param name="ct"></param>
    public void SetAutoPlayNext(bool isAuto, CancellationToken ct)
    {
        _videoDisposables = DisposeEvent(_videoDisposables);

        if(!isAuto)
        {
            return;
        }

        _videoPlayer.IsFinishVideo
            .TakeUntilDestroy(this)
            .Where(value => value)
            .Subscribe(value =>
            {
                PlayNext();
            }).AddTo(_videoDisposables);
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
        Vector2 mousePos = Vector2.zero;

        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Where(_ => !EventSystem.current.IsPointerOverGameObject() && _videoUIPresenter.IsShow)
            .Subscribe(async _ =>
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, Input.mousePosition, _camera, out mousePos);

                if(_canvasRectTransform.rect.Contains(mousePos))
                {
                    await _videoUIPresenter.ShowAsync(Ct);
                }
                else
                {
                    await _videoUIPresenter.HideAsync(Ct);
                    AudioManager.Instance.SaveVolume();
                }
            }).AddTo(_pointerDisposables);
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
                        await _onMonitorUI.HideAsync(ct);
                        _videoPlayer.AudioSource.spatialBlend = 1;
                        await PlayAsync(ct);
                        break;
                    case MuseumState.Monitor:
                        await _onMonitorUI.ShowAsync(ct);
                        _videoPlayer.AudioSource.spatialBlend = 0;
                        await PlayAsync(ct);
                        break;
                    case MuseumState.Pause:
                        Pause();
                        break;
                    default:
                        SetMute(true);
                        break;
                }
            });
    }

    private void DisposePointerEvent()
    {
        if(_pointerDisposables == new CompositeDisposable())
        {
            return;
        }

        _pointerDisposables = DisposeEvent(_pointerDisposables);
    }
}

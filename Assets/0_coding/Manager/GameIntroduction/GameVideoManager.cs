using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameVideoManager : ProductionManagerBase<GameVideoManager>
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
    private VideoUIPresenter _videoUI;
    [Header("キャプションUI")]
    [SerializeField]
    private GameCaptionPresenter _captionUI;
    [Header("モニターの範囲検知用画像")]
    [SerializeField]
    private Image _rangeImage;
    [Header("ローディングUI")]
    [SerializeField]
    private CircleLoadingPresenter _loadingUI;
    [Header("モニター上のUI")]
    [SerializeField]
    private OnMonitorPresenter _onMonitorUI;


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
    private Camera _camera;
    private RectTransform _canvasRectTransform;
    private CompositeDisposable _videoDisposables = new CompositeDisposable();
    private CompositeDisposable _pointerDisposables = new CompositeDisposable();

    protected override void Init()
    {
        _camera = _videoCanvas.worldCamera;
        _canvasRectTransform = _videoCanvas.GetComponent<RectTransform>();
        _rangeImage.color -= new Color(0, 0, 0, 1);
        _rangeImage.enabled = false;
    }

    protected override void SetEvent()
    {
        SetEventTarget(Ct);
        SetEventVideo(Ct);
        SetEventVideoPlay(Ct);
    }

    /// <summary>
    /// モニターにターゲット時のイベント設定
    /// </summary>
    private void SetEventTarget(CancellationToken ct)
    {
        GameStateManager.MuseumStatus
            .TakeUntilDestroy(this)
            .Select(value => value == MuseumState.Monitor)
            .SkipWhile(value => !value)
            .DistinctUntilChanged()
            .Subscribe(async value =>
            {
                if (value)
                {
                    await TargetAsync(_monitor, ct);
                    SetEventPointer();
                    _videoUI.SetPlayButton(value);
                    _rangeImage.enabled = true;
                }
                else
                {
                    DisposePointerEvent();
                    _rangeImage.enabled = false;
                    await ClearTargetAsync(ct);
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
            .Where(_ => !EventSystem.current.IsPointerOverGameObject() && _videoUI.IsShow)
            .Subscribe(async _ =>
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, Input.mousePosition, _camera, out mousePos);

                if (_canvasRectTransform.rect.Contains(mousePos))
                {
                    await _videoUI.ShowAsync(Ct);
                }
                else
                {
                    await _videoUI.HideAsync(Ct);
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
                        await PauseAsync(ct);
                        break;
                    default:
                        SetMute(true);
                        break;
                }
            });
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
                GameData targetGameData = _gameDatas.GetGameData(value);

                _loadingUI.ShowAsync(ct).Forget();
                await _videoPlayer.PlayAsync(targetGameData.YoutubeURL, ct);
                SetGameDataToUI(targetGameData);
                _videoUI.SetPlayButton(true);
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
        _captionUI.SetExplain(gameData.Description);
    }

    /// <summary>
    /// 動画再生
    /// </summary>
    public async UniTask PlayAsync(CancellationToken ct)
    {
        await _videoPlayer.PlayAsync(ct);
    }

    /// <summary>
    /// 次の動画を再生
    /// </summary>
    public async UniTask PlayNextAsync(CancellationToken ct)
    {
        await PauseAsync(ct);

        if (_videoIndex.Value == _gameDatas.GetCount() - 1)
        {
            _videoIndex.Value = 0;
            return;
        }

        _videoIndex.Value++;
    }

    /// <summary>
    /// 動画停止
    /// </summary>
    public async UniTask PauseAsync(CancellationToken ct)
    {
        await _videoPlayer.PauseAsync(ct);
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
    /// 動画の自動再生設定
    /// </summary>
    /// <param name="isAuto"> 自動再生するか </param>
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
            .Subscribe(async value =>
            {
                await PlayNextAsync(ct);
            }).AddTo(_videoDisposables);
    }

    /// <summary>
    /// イベント削除
    /// </summary>
    private void DisposePointerEvent()
    {
        if(_pointerDisposables == new CompositeDisposable())
        {
            return;
        }

        _pointerDisposables = DisposeEvent(_pointerDisposables);
    }
}

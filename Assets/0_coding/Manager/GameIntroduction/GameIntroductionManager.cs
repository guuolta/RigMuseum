using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

public class GameIntroductionManager : SingletonObjectBase<GameIntroductionManager>
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
    [Header("アニメーションの時間")]
    [SerializeField]
    private float _animationTime = 0.1f;
    [Header("モニターにターゲット時のモニターとプレイヤーの距離")]
    [SerializeField]
    private float _targetDistance = 10f;
    [Header("モニターにターゲット解除時のモニターとプレイヤーの距離")]
    [SerializeField]
    private float _targetClearDistance = 15f;

    private Vector3 _targetPos = Vector3.zero;
    private Vector3 _targetRot = Vector3.zero;
    private Vector3 _clearPos = Vector3.zero;


    protected override void Init()
    {
        _targetPos = _monitor.Transform.position + (_monitor.Transform.right * _targetDistance);
        _targetRot = _monitor.Transform.localEulerAngles + new Vector3(0, -90, 0);
        _clearPos = _targetPos + (_monitor.Transform.right * _targetClearDistance);
        _videoPlayer.Play(gameDatas.GetGameYoutubeURL(0)).Forget();
    }

    protected override void SetEvent()
    {
        SetEventTarget();
        SetEventVideo();
    }

    /// <summary>
    /// モニターにターゲット時のアニメーションイベント設定
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

                    await PlayerManager.Instance.TargetObjectAsync(_animationTime, _targetPos, _targetRot);
                }
                else
                {
                    await PlayerManager.Instance.ClearTargetAsync(_animationTime, _clearPos);
                }
            });
    }

    /// <summary>
    /// 各ステートにおける動画の設定
    /// </summary>
    private void SetEventVideo()
    {
        GameStateManager.MuseumStatus
            .Skip(1)
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                switch (value)
                {
                    case MuseumState.Play:
                        _videoPlayer.AudioSource.spatialBlend = 1;
                        _videoPlayer.Play();
                        break;
                    case MuseumState.Monitor:
                        _videoPlayer.AudioSource.spatialBlend = 0;
                        _videoPlayer.Play();
                        break;
                    case MuseumState.Pause:
                        _videoPlayer.Pause();
                        break;
                    default:
                        _videoPlayer.AudioSource.mute = true;
                        break;
                }
            });
    }
}

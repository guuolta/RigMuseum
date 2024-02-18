using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
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


    public override void Init()
    {
        _targetPos = _monitor.Transform.position + (_monitor.Transform.right * _targetDistance);
        _targetRot = _monitor.Transform.localEulerAngles + new Vector3(0, -90, 0);
        _clearPos = _targetPos + (_monitor.Transform.right * _targetClearDistance);
        _videoPlayer.SetInitVideo(gameDatas.GetGameYoutubeURL(0)).Forget();
    }

    public override void SetEvent()
    {
        SetEventTarget();
    }

    /// <summary>
    /// モニターにターゲット時のアニメーションイベント設定
    /// </summary>
    private void SetEventTarget()
    {
        GameStateManager.MuseumStatus
            .Skip(1)
            .Select(value => value == MuseumState.Monitor)
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
            }).AddTo(this);
    }
}

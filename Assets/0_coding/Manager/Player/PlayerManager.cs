using Cysharp.Threading.Tasks;
using System;
using UniRx;
using UnityEditor.PackageManager;
using UnityEngine;


public class PlayerManager : SingletonObjectBase<PlayerManager>
{
    [Header("プレイヤー")]
    [SerializeField]
    private Player _player;
    [Header("プレイヤーの操作")]
    [SerializeField]
    private PlayerOperater _playerOperater;

    private bool _isReverseVertical = false;
    private bool _isReverseHorizontal = false;
    private float _moveSpeed;
    private float _rotateSpeed;

    public override void Init()
    {
        GetSettings();
        _playerOperater.SetInit(_player);
    }

    public override void SetEvent()
    {
        SetEventPlayerOperation();
        _playerOperater.SetEventKey(_moveSpeed);
        _playerOperater.SetEventMouse(_isReverseVertical, _isReverseHorizontal, _rotateSpeed);
    }

    /// <summary>
    /// プレイヤーの操作設定
    /// </summary>
    private void SetEventPlayerOperation()
    {
        GameStateManager.MuseumStatus
            .Skip(1)
            .Select(value => value == MuseumState.Play)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                if (value)
                {
                    _playerOperater.SetEventKey(GetMoveSpeed());
                    _playerOperater.SetEventMouse(_isReverseVertical, _isReverseHorizontal, GetSensitivity());
                }
                else
                {
                    _playerOperater.DisposeEventPlayerOperation();
                }
            }).AddTo(this);
    }


    /// <summary>
    /// プレイヤーの操作設定をセーブデータから取得
    /// </summary>
    private void GetSettings()
    {
        _moveSpeed = SaveManager.GetMoveSpeed();
        _rotateSpeed = SaveManager.GetSensitivity();
        _isReverseVertical = SaveManager.GetIsVerticalReverse();
        _isReverseHorizontal= SaveManager.GetIsHorizontalReverse();
    }

    /// <summary>
    /// プレイヤーの動くスピード取得
    /// </summary>
    /// <returns></returns>
    private float GetMoveSpeed()
    {
        return _moveSpeed * 0.5f;
    }

    /// <summary>
    /// マウスの感度取得
    /// </summary>
    /// <returns></returns>
    private float GetSensitivity()
    {
        return _rotateSpeed * 0.5f;
    }

    /// <summary>
    /// マウス感度設定
    /// </summary>
    /// <param name="value">感度</param>
    public void SetMoveSpeed(float value)
    {
        _moveSpeed = value;
    }

    /// <summary>
    /// マウス感度設定
    /// </summary>
    /// <param name="value">感度</param>
    public void SetSensitivity(float value)
    {
        _rotateSpeed = value;
    }

    /// <summary>
    /// マウスの縦操作反転かどうか
    /// </summary>
    /// <param name="IsReverse">反転させるか</param>
    public void SetIsReveseVertical(bool IsReverse)
    {
        _isReverseVertical = IsReverse;
    }

    /// <summary>
    /// マウスの横操作反転かどうか
    /// </summary>
    /// <param name="IsReverse">反転させるか</param>
    public void SetIsReverseHorizontal(bool IsReverse)
    {
        _isReverseHorizontal = IsReverse;
    }

    /// <summary>
    /// プレイヤーの設定を保存
    /// </summary>
    public void SaveSetting()
    {
        SaveManager.SetSaveMoveSpeed(_moveSpeed);
        SaveManager.SetSaveSensitivity(_rotateSpeed);
        SaveManager.SetSaveIsVerticalReverse(_isReverseVertical);
        SaveManager.SetSaveIsHorizontalReverse(_isReverseHorizontal);
    }

    /// <summary>
    /// プレイヤーを目的地に向かわせる
    /// </summary>
    /// <param name="animationTime"> アニメーションの時間 </param>
    /// <param name="pos"> 目的地 </param>
    /// <param name="rot"> 目的回転値 </param>
    /// <returns></returns>
    public async UniTask TargetObjectAsync(float animationTime, Vector3 pos, Vector3 rot)
    {
        await _playerOperater.MovePlayerAsync(animationTime, pos, rot, DG.Tweening.Ease.InQuad);
    }

    /// <summary>
    /// ターゲット状態を解除
    /// </summary>
    /// <param name="animationTime"> アニメーションの時間 </param>
    /// <param name="pos"> 解除時の位置 </param>
    /// <returns></returns>
    public async UniTask ClearTargetAsync(float animationTime, Vector3 pos)
    {
        await _playerOperater.MovePlayerAsync(animationTime, pos, _player.LocalEulerAngles, DG.Tweening.Ease.OutQuad);
    }
}

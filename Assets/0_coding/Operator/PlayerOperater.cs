using System;
using UniRx;
using UnityEngine;

/// <summary>
/// プレイヤーの動き
/// </summary>
public class PlayerOperater : SingletonObjectBase<PlayerOperater>
{
    private bool _isReverseVertical = false;
    private bool _isReverseHorizontal = false;
    [Header("プレイヤー")]
    [SerializeField]
    private Player _player;
    [Header("カメラが移動するスピード")]
    [SerializeField]
    private float _moveSpeed;
    [Header("カメラが回転するスピード")]
    [SerializeField]
    private float _rotateSpeed;
    [Header("カメラの回転上限")]
    [Range(0f, 180f)]
    [SerializeField]
    private float _upLimitRotation = 90;
    [Header("カメラの回転下限")]
    [Range(-180f, 0f)]
    [SerializeField]
    private float _downLimitRotation = -90;
    [Header("上昇移動のキー")]
    [SerializeField]
    private KeyCode _upMoveKey = KeyCode.Space;
    [Header("下降移動のキー")]
    [SerializeField]
    private KeyCode _downMoveKey = KeyCode.LeftShift;
    [Header("オプションキー")]
    [SerializeField]
    private KeyCode _optionKey = KeyCode.Tab;
    private Transform _transform;
    private Rigidbody _rb;
    private CompositeDisposable _keyEventDisposes = new CompositeDisposable();
    private CompositeDisposable _mouseEventDisposes = new CompositeDisposable();

    public override void Init()
    {
        base.Init();
        _transform = _player.Transform;
        _rb = _player.Rigidbody;
    }

    public override void SetEvent()
    {
        SetEventPlayerOperation();
        SetEventOptionKey();
    }

    private void OnDestroy()
    {
        DisposeEventPlayerOperation();
    }

    /// <summary>
    /// プレイヤーの操作設定
    /// </summary>
    private void SetEventPlayerOperation()
    {
        GameStateManager.MuseumStatus
            .Select(value => value == MuseumState.Play)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                if(value)
                {
                    SetEventKey();
                    SetMouseEvent();
                }
                else
                {
                    DisposeEventPlayerOperation();
                }
            }).AddTo(this);
    }

    /// <summary>
    /// イベント削除
    /// </summary>
    private CompositeDisposable DisposeEvent(CompositeDisposable disposable)
    {
        disposable.Dispose();
        return new CompositeDisposable();
    }

    /// <summary>
    /// キー、マウス操作削除
    /// </summary>
    private void DisposeEventPlayerOperation()
    {
        _keyEventDisposes = DisposeEvent(_keyEventDisposes);
        _mouseEventDisposes = DisposeEvent(_mouseEventDisposes);

        if(_rb != null)
        {
            _rb.velocity = Vector3.zero;
        }
    }
    
    /// <summary>
    /// キー操作設定
    /// </summary>
    private void SetEventKey()
    {
        /*前方向*/
        float frontDir = 0;
        /*横方向*/
        float rightDir = 0;
        /*上方向*/
        float upDir = 0;
        /*下方向*/
        float downDir = 0;

        /*前後どちらか*/
        Observable.EveryUpdate()
            .Where(_ => Input.GetAxis("Vertical") != 0)
            .Select(direction => Input.GetAxis("Vertical"))
            .Subscribe(direction => frontDir = direction)
            .AddTo(_keyEventDisposes);

        /*左右どちらか*/
        Observable.EveryUpdate()
            .Where(_ => Input.GetAxis("Horizontal") != 0)
            .Select(direction => Input.GetAxis("Horizontal"))
            .Subscribe(direction => rightDir = direction)
            .AddTo(_keyEventDisposes);

        /*上方向か*/
        Observable.EveryUpdate()
            .Where(_ => Input.GetKey(_upMoveKey))
            .Subscribe(_ => upDir = 1)
            .AddTo(_keyEventDisposes);
        
        /*上方向解除*/
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyUp(_upMoveKey))
            .Subscribe(_ => upDir = 0)
            .AddTo(_keyEventDisposes);

        /*下方向か*/
        Observable.EveryUpdate()
            .Where(_ => Input.GetKey(_downMoveKey))
            .Subscribe(_ => downDir = 1)
            .AddTo(_keyEventDisposes);

        /*下方向解除*/
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyUp(_downMoveKey))
            .Subscribe(_ => downDir = 0)
            .AddTo(_keyEventDisposes);

        /*移動処理*/
        Observable.EveryUpdate()
            .Where(_ => Input.anyKey)
            .Subscribe(_ =>
            {
                _rb.velocity = ( frontDir * DeleateMoveValueY(_transform.forward)
                    + rightDir * DeleateMoveValueY(_transform.right) 
                    + upDir * new Vector3(0f, 1f, 0f)
                    + downDir * new Vector3(0f, -1f, 0f) )
                    * _moveSpeed;
            }).AddTo(_keyEventDisposes);

        /*初期化処理*/
        Observable.EveryUpdate()
            .Where(_ => !Input.anyKey)
            .Subscribe(_ =>
            {
                _rb.velocity = Vector3.zero;
                frontDir = 0;
                rightDir = 0;
            }).AddTo(_keyEventDisposes);
    }

    /// <summary>
    /// マウス操作設定
    /// </summary>
    private void SetMouseEvent()
    {
        /*横方向の視点移動*/
        Observable.EveryUpdate()
            .Select(direction => Input.GetAxis("Mouse X"))
            .DistinctUntilChanged()
            .Subscribe(direction =>
            {
                Vector3 rot = _transform.localEulerAngles;
                rot.y = _isReverseHorizontal ? (rot.y - _rotateSpeed * direction) : (rot.y + _rotateSpeed * direction);
                _transform.localEulerAngles = rot;
            }).AddTo(_mouseEventDisposes);

        /*縦方向の視点移動*/
        Observable.EveryUpdate()
            .Select(direction => Input.GetAxis("Mouse Y"))
            .DistinctUntilChanged()
            .Subscribe(direction =>
            {
                Vector3 rot = _transform.localEulerAngles;
                rot.x = rot.x > 180f ? (rot.x - 360f)
                    : rot.x < -180f ? (rot.x + 360f)
                    : rot.x;
                rot.x = _isReverseVertical ? (rot.x + _rotateSpeed * direction) : (rot.x - _rotateSpeed * direction);
                rot.x = Mathf.Clamp(rot.x, _downLimitRotation, _upLimitRotation);
                _transform.localEulerAngles = rot;
            }).AddTo(_mouseEventDisposes);
    }

    /// <summary>
    /// オプションキー設定
    /// </summary>
    private void SetEventOptionKey()
    {
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(_optionKey))
            .Subscribe(_ =>
            {
                GameStateManager.TogglePauseState();
            }).AddTo(this);
    }

    /*縦方向の移動削除*/
    private Vector3 DeleateMoveValueY(Vector3 value)
    {
        value.y = 0;
        return value;
    }

    /// <summary>
    /// マウス感度設定
    /// </summary>
    /// <param name="value">感度</param>
    public void SetSensitivity(float value)
    {
        _rotateSpeed = value * 0.5f;
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
}

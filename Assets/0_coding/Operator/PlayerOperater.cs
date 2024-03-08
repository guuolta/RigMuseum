using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// プレイヤーの動き
/// </summary>
public class PlayerOperater : ObjectBase
{
    [Header("マウスのクリックの範囲")]
    [Range(10f, 30f)]
    [SerializeField]
    private float _clickRange = 10f;
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
    private Camera _camera;
    private CompositeDisposable _keyEventDisposes = new CompositeDisposable();
    private CompositeDisposable _mouseEventDisposes = new CompositeDisposable();

    protected override void SetEvent()
    {
        SetEventObjectClick();
        SetEventOptionKey();
    }

    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="player"> プレイヤー </param>
    public void SetInit(Player player)
    {
        _transform = player.transform;
        _rb = player.Rigidbody;
        _camera = player.Camera;
    }

    /// <summary>
    /// キー操作設定
    /// </summary>
    public void SetEventKey(float speed)
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
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetAxis("Vertical") != 0)
            .Select(direction => Input.GetAxis("Vertical"))
            .Subscribe(direction => frontDir = direction)
            .AddTo(_keyEventDisposes);

        /*左右どちらか*/
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetAxis("Horizontal") != 0)
            .Select(direction => Input.GetAxis("Horizontal"))
            .Subscribe(direction => rightDir = direction)
            .AddTo(_keyEventDisposes);

        /*上方向か*/
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetKey(_upMoveKey))
            .Subscribe(_ => upDir = 1)
            .AddTo(_keyEventDisposes);
        
        /*上方向解除*/
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetKeyUp(_upMoveKey))
            .Subscribe(_ => upDir = 0)
            .AddTo(_keyEventDisposes);

        /*下方向か*/
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetKey(_downMoveKey))
            .Subscribe(_ => downDir = 1)
            .AddTo(_keyEventDisposes);

        /*下方向解除*/
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetKeyUp(_downMoveKey))
            .Subscribe(_ => downDir = 0)
            .AddTo(_keyEventDisposes);

        /*移動処理*/
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Where(_ => Input.anyKey)
            .Subscribe(_ =>
            {
                _rb.velocity = ( frontDir * DeleateMoveValueY(_transform.forward)
                    + rightDir * DeleateMoveValueY(_transform.right) 
                    + upDir * new Vector3(0f, 1f, 0f)
                    + downDir * new Vector3(0f, -1f, 0f) )
                    * speed;
            }).AddTo(_keyEventDisposes);

        /*初期化処理*/
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
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
    public void SetEventMouse(bool isReverseVertical, bool isReverseHorizontal, float speed)
    {
        /*横方向の視点移動*/
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Select(direction => Input.GetAxis("Mouse X"))
            .DistinctUntilChanged()
            .Subscribe(direction =>
            {
                Vector3 rot = _transform.localEulerAngles;
                rot.y = isReverseHorizontal ? (rot.y - speed * direction) : (rot.y + speed * direction);
                _transform.localEulerAngles = rot;
            }).AddTo(_mouseEventDisposes);

        /*縦方向の視点移動*/
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Select(direction => Input.GetAxis("Mouse Y"))
            .DistinctUntilChanged()
            .Subscribe(direction =>
            {
                Vector3 rot = _transform.localEulerAngles;
                rot.x = rot.x > 180f ? (rot.x - 360f)
                    : rot.x < -180f ? (rot.x + 360f)
                    : rot.x;
                rot.x = isReverseVertical ? (rot.x + speed * direction) : (rot.x - speed * direction);
                rot.x = Mathf.Clamp(rot.x, _downLimitRotation, _upLimitRotation);
                _transform.localEulerAngles = rot;
            }).AddTo(_mouseEventDisposes);

        
    }

    /// <summary>
    /// オブジェクトのクリックの処理設定
    /// </summary>
    private void SetEventObjectClick()
    {
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Where(_ => GameStateManager.MuseumStatus.Value != MuseumState.Pause && Input.GetMouseButtonDown(0))
            .DistinctUntilChanged()
            .Subscribe(_ =>
            {
                RaycastHit hit;

                if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit, _clickRange))
                {
                    var obj = hit.collider.gameObject.GetComponent<TouchObjectBase>();
                    if (obj != null)
                    {
                        obj.StartEvent();
                    }
                }
            });
    }

    /// <summary>
    /// オプションキー設定
    /// </summary>
    private void SetEventOptionKey()
    {
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetKeyDown(_optionKey))
            .DistinctUntilChanged()
            .Subscribe(_ =>
            {
                GameStateManager.TogglePauseState();
            });
    }

    /// <summary>
    /// キー、マウス操作削除
    /// </summary>
    public void DisposeEventPlayerOperation()
    {
        _keyEventDisposes = DisposeEvent(_keyEventDisposes);
        _mouseEventDisposes = DisposeEvent(_mouseEventDisposes);

        if (_rb != null)
        {
            _rb.velocity = Vector3.zero;
        }
    }

    /// <summary>
    /// 縦方向の移動削除
    /// </summary>
    /// <param name="value"> 移動距離 </param>
    /// <returns></returns>
    private Vector3 DeleateMoveValueY(Vector3 value)
    {
        value.y = 0;
        return value;
    }

    /// <summary>
    /// プレイヤーを移動
    /// </summary>
    /// <param name="animationTime"> アニメーションの時間 </param>
    /// <param name="pos"> 目的地 </param>
    /// <param name="rot"> 目的回転値 </param>
    /// <param name="ease"> easeのタイプ </param>
    /// <returns></returns>
    public async UniTask MovePlayerAsync(float animationTime, Vector3 pos, Vector3 rot, Ease ease, CancellationToken ct)
    {
        _transform.DOComplete();
        var sequence = DOTween.Sequence();

        await sequence
            .Append(_transform.DOMove(pos, animationTime).SetEase(ease))
            .Join(_transform.DORotate(rot, animationTime).SetEase(ease))
            .ToUniTask(cancellationToken: ct);
    }

    ////UI検査用
    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Debug.Log("ok");
    //        // マウスポインターの位置にあるUI要素を取得
    //        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
    //        pointerEventData.position = Input.mousePosition;

    //        // RaycastでUI要素をチェックし、結果をリストに格納
    //        List<RaycastResult> results = new List<RaycastResult>();
    //        EventSystem.current.RaycastAll(pointerEventData, results);

    //        // RaycastでヒットしたUI要素の名前を取得
    //        if (results.Count > 0)
    //        {
    //            Debug.Log("UI Name: " + results[0].gameObject.name);
    //        }
    //    }
    //}
}
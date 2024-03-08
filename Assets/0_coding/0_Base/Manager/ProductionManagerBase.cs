using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class ProductionManagerBase<T> : SingletonObjectBase<T>
    where T : ObjectBase
{
    [Header("アニメーション時間")]
    [SerializeField]
    private float _animationTime = 0.1f;
    protected float animationTime => _animationTime;
    [Header("作品との距離")]
    [SerializeField]
    private float _distance = 20f;
    protected float distance => _distance;
    [Header("解除時の作品との距離")]
    [SerializeField]
    private float _clearDistance = 25f;
    protected float clearDistance => _clearDistance;

    private TouchObjectBase _targetObject = null;

    /// <summary>
    /// オブジェクトにターゲットする
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask TargetAsync(TouchObjectBase obj, CancellationToken ct)
    {
        if(_targetObject != null)
        {
            _targetObject.ClearTouch();
        }

        _targetObject = obj;
        await PlayerManager.Instance.TargetObjectAsync(animationTime,
            GetCameraPos(obj.Transform.position, GetDirection(obj.Transform, obj.Direction), distance),
            GetCameraRot(obj.Transform.localEulerAngles, GetAddRotation(obj.Direction)),
            ct);
    }

    /// <summary>
    /// オブジェクトにターゲットする
    /// </summary>
    /// <param name="distance"> カメラとオブジェクトの距離 </param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask TargetAsync(TouchObjectBase obj, float distance, CancellationToken ct)
    {
        if (_targetObject != null)
        {
            _targetObject.ClearTouch();
        }

        _targetObject = obj;
        await PlayerManager.Instance.TargetObjectAsync(animationTime,
            GetCameraPos(obj.Transform.position, GetDirection(obj.Transform, obj.Direction), distance),
            GetCameraRot(obj.Transform.localEulerAngles, GetAddRotation(obj.Direction)),
            ct);
    }

    /// <summary>
    /// ターゲット状態を解除
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask ClearTargetAsync(CancellationToken ct)
    {
        await PlayerManager.Instance.ClearTargetAsync(animationTime,
            GetCameraPos(_targetObject.Transform.position, GetDirection(_targetObject.Transform, _targetObject.Direction), clearDistance),
            ct);
        _targetObject.ClearTouch();
        _targetObject = null;
    }

    /// <summary>
    /// ターゲット状態を解除
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask ClearTargetAsync(float distance, CancellationToken ct)
    {
        await PlayerManager.Instance.ClearTargetAsync(animationTime,
            GetCameraPos(_targetObject.Transform.position, GetDirection(_targetObject.Transform, _targetObject.Direction), distance),
            ct);
        _targetObject.ClearTouch();
        _targetObject = null;
    }

    /// <summary>
    /// 移動後のカメラの位置を取得
    /// </summary>
    /// <param name="pos"> 対象のオブジェクトの位置 </param>
    /// <param name="direction"> 対象のオブジェクトから移動する方向 </param>
    /// <returns></returns>
    protected Vector3 GetCameraPos(Vector3 pos ,Vector3 direction, float distance)
    {
        return pos + direction * distance; 
    }

    /// <summary>
    /// 移動後のカメラの角度を取得
    /// </summary>
    /// <param name="rot"> 対象のオブジェクトの角度 </param>
    /// <param name="addRot"> 追加する角度 </param>
    /// <returns></returns>
    protected Vector3 GetCameraRot(Vector3 rot, Vector3 addRot)
    {
        return rot + addRot;
    }

    /// <summary>
    /// 方向を取得
    /// </summary>
    /// <param name="transform"> 対象 </param>
    /// <param name="direction"> 方向 </param>
    /// <returns></returns>
    protected Vector3 GetDirection(Transform transform, TargetDirection direction)
    {
        switch (direction)
        {
            case TargetDirection.Fornt:
                return transform.forward;
            case TargetDirection.Back:
                return -transform.forward;
            case TargetDirection.Right:
                return transform.right;
            case TargetDirection.Left:
                return -transform.right;
            case TargetDirection.Up:
                return transform.up;
            case TargetDirection.Down:
                return -transform.up;
            default:
                return Vector3.zero;
        }
    }

    /// <summary>
    /// 追加する角度を取得
    /// </summary>
    /// <param name="direction"> 方向 </param>
    /// <returns></returns>
    protected Vector3 GetAddRotation(TargetDirection direction)
    {
        switch (direction)
        {
            case TargetDirection.Fornt:
                return new Vector3(0, 180, 0);
            case TargetDirection.Back:
                return new Vector3(0, 0, 0);
            case TargetDirection.Right:
                return new Vector3(0, -90, 0);
            case TargetDirection.Left:
                return new Vector3(0, 90, 0);
            case TargetDirection.Up:
                return new Vector3(90, 0, 0);
            case TargetDirection.Down:
                return new Vector3(-90, 0, 0);
            default:
                return Vector3.zero;
        }
    }
}

public enum TargetDirection
{
    None,
    Fornt,
    Back,
    Right,
    Left,
    Up,
    Down
}
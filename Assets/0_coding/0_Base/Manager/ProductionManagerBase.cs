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
    private float _distance = 10f;
    protected float distance => _distance;
    [Header("解除時の作品との距離")]
    [SerializeField]
    private float _clearDistance = 15f;
    protected float clearDistance => _clearDistance;

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
}

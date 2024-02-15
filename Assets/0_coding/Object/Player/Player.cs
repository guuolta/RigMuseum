using UnityEngine;

/// <summary>
/// プレイヤー
/// </summary>
public class Player : GameObjectBase
{
    [Header("プレイヤーのカメラ")]
    [SerializeField]
    private Camera _camera;
    /// <summary>
    /// プレイヤーのカメラ
    /// </summary>
    public Camera Camera => _camera;

    private BoxCollider _collider;
    /// <summary>
    /// 当たり判定
    /// </summary>
    public BoxCollider Collider
    {
        get
        {
            if(_collider == null)
            {
                _collider = GetComponent<BoxCollider>();
            }

            return _collider;
        }
    }

    private Rigidbody _rb;
    /// <summary>
    /// 物理演算
    /// </summary>
    public Rigidbody Rigidbody
    {
        get
        {
            if(_rb == null)
            {
                _rb = GetComponent<Rigidbody>();
            }

            return _rb;
        }
    }
}

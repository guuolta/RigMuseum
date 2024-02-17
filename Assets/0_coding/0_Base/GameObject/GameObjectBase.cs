using UnityEngine;

/// <summary>
/// ゲームオブジェクトのベース
/// </summary>
public class GameObjectBase : ObjectBase
{
    private GameObject _gameObject;
    /// <summary>
    /// ゲームオブジェクト
    /// </summary>
    public GameObject GameObject
    {
        get
        {
            if (_gameObject == null)
            {
                _gameObject = gameObject;
            }

            return _gameObject;
        }
    }

    private Transform _transform;
    /// <summary>
    /// トランスフォーム
    /// </summary>
    public Transform Transform
    {
        get
        {
            if (_transform == null)
            {
                _transform = transform;
            }

            return _transform;
        }
    }

    /// <summary>
    /// 前方向
    /// </summary>
    public Vector3 Forward
    {
        get
        {
            return Transform.forward;
        }
    }
    
    /// <summary>
    /// 右方向
    /// </summary>
    public Vector3 Right
    {
        get
        {
            return Transform.right;
        }
    }

    /// <summary>
    /// 上方向
    /// </summary>
    public Vector3 Up
    {
        get
        {
            return Transform .up;
        }
    }

    public Vector3 LocalEulerAngles
    {
        get
        {
            return Transform.localEulerAngles;
        }
    }
}

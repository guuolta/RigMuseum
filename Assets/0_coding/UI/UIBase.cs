using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI系のベース
/// </summary>
public class UIBase : MonoBehaviour
{
    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        SetFirstEvent();
        SetEvent();
    }

    /// <summary>
    /// 変数の初期化など
    /// </summary>
    public virtual void Init()
    {

    }

    /// <summary>
    /// 先に行いたいイベントの発行
    /// </summary>
    public virtual void SetFirstEvent()
    {

    }

    /// <summary>
    /// イベントの発行
    /// </summary>
    public virtual void SetEvent()
    {

    }

    private GameObject _gameObject;
    public GameObject GameObject
    {
        get
        {
            if(_gameObject == null)
            {
                _gameObject = gameObject;
            }

            return _gameObject;
        }
    }

    private Transform _transform;
    public Transform Transform
    {
        get
        {
            if(_transform == null)
            {
                _transform = transform;
            }

            return _transform;
        }
    }

    private RectTransform _rectTransform;
    public RectTransform RectTransform
    {
        get
        {
            if(_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }

            return _rectTransform;
        }
    }
}

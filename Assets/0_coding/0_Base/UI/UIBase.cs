using UnityEngine;

/// <summary>
/// UI系のベース
/// </summary>
public class UIBase : GameObjectBase
{
    [Header("ボタンが押せないときの透明度")]
    [Range(0f, 1f)]
    [SerializeField]
    private float _disInteractiveColor = 0.8f;

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

    private CanvasGroup _canvasGroup;
    public CanvasGroup CanvasGroup
    {
        get
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
                if (_canvasGroup == null)
                {
                    _canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }

            return _canvasGroup;
        }
    }

    /// <summary>
    /// UIを押せるようにするか設定
    /// </summary>
    /// <param name="isInteractive">押せるか</param>
    public void ChangeInteractive(bool isInteractive)
    {
        if (_canvasGroup == null)
        {
            return;
        }

        _canvasGroup.interactable = isInteractive;
        _canvasGroup.blocksRaycasts = isInteractive;
        if (isInteractive)
        {
            _canvasGroup.alpha = 1f;
        }
        else
        {
            _canvasGroup.alpha = _disInteractiveColor;
        }
    }
}

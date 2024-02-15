using UnityEngine;

/// <summary>
/// UI系のベース
/// </summary>
public class UIBase : GameObjectBase
{
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

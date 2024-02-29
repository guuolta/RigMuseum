using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class MonitorCursorDetector : UIBase,
    IPointerEnterHandler,
    IPointerExitHandler
{
    private BoolReactiveProperty _isEnter = new BoolReactiveProperty(false);
    public BoolReactiveProperty IsEnter => _isEnter;

    protected override void Init()
    {
        ChangeInteractive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isEnter.Value = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isEnter.Value = false;
    }
}

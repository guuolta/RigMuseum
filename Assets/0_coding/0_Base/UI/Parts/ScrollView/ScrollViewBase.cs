using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewBase : UIPartBase
{
    private ScrollRect _scrollRect;
    public ScrollRect ScrollRect
    {
        get
        {
            if(_scrollRect == null)
            {
                _scrollRect = GetComponent<ScrollRect>();
            }

            return _scrollRect;
        }
    }

    private Scrollbar _scrollbar;
    public Scrollbar Scrollbar
    {
        get
        {
            if(_scrollbar == null)
            {
                _scrollbar = ScrollRect.verticalScrollbar;
            }

            return _scrollbar;
        }
    }

    private Image _handle;
    public Image Handle
    {
        get
        {
            if(_handle == null)
            {
                _handle = Scrollbar.handleRect.GetComponent<Image>();
            }

            return _handle;
        }
    }

    public override void SetEvent()
    {
        SetEventPointer(Handle);
    }
}

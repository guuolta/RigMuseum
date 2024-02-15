using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePanelView : PanelViewBase
{
    [Header("感度設定UI")]
    [SerializeField]
    private ValueUIPart _sensitivityUI;

    /// <summary>
    /// 感度設定UI
    /// </summary>
    public ValueUIPart SensitivityUI => _sensitivityUI;
    
    [Header("上下反転トグル")]
    [SerializeField]
    private ToggleBase _isVirticulToggle;
    /// <summary>
    /// 上下反転トグル
    /// </summary>
    public ToggleBase VirticulToggle => _isVirticulToggle;

    [Header("左右反転トグル")]
    [SerializeField]
    private ToggleBase _isHorizontalToggle;
    /// <summary>
    /// 左右反転トグル
    /// </summary>
    public ToggleBase HorizontalToggle => _isHorizontalToggle;
}

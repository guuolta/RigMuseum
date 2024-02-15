using UnityEngine;

public class PlayerSettingPanelView : PanelViewBase
{
    [Header("プレイヤーのスピード設定UI")]
    [SerializeField]
    private ValueUIPart _moveSpeedUI;

    /// <summary>
    /// プレイヤーのスピード設定UI
    /// </summary>
    public ValueUIPart MoveSpeedUI => _moveSpeedUI;

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
    public ToggleBase VerticulToggle => _isVirticulToggle;

    [Header("左右反転トグル")]
    [SerializeField]
    private ToggleBase _isHorizontalToggle;
    /// <summary>
    /// 左右反転トグル
    /// </summary>
    public ToggleBase HorizontalToggle => _isHorizontalToggle;
}

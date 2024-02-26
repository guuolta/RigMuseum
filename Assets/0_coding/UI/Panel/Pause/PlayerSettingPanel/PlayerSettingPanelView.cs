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
    
    [Header("上下反転チェックボックス")]
    [SerializeField]
    private CheckBoxBase _isVirticulCheckBox;
    /// <summary>
    /// 上下反転チェックボックス
    /// </summary>
    public CheckBoxBase VerticulCheckBox => _isVirticulCheckBox;

    [Header("左右反転チェックボックス")]
    [SerializeField]
    private CheckBoxBase _isHorizontalCheckBox;
    /// <summary>
    /// 左右反転チェックボックス
    /// </summary>
    public CheckBoxBase HorizontalCheckBox => _isHorizontalCheckBox;
}

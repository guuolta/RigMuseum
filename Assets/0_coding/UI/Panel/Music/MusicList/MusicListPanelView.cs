using TMPro;
using UnityEngine;

public class MusicListPanelView : PanelViewBase
{
    [Header("番号テキスト")]
    [SerializeField]
    private TMP_Text idText;
    /// <summary>
    /// 番号テキスト
    /// </summary>
    public TMP_Text IDText => idText;
    [Header("タイトルテキスト")]
    [SerializeField]
    private TMP_Text titleText;
    /// <summary>
    /// タイトルテキスト
    /// </summary>
    public TMP_Text TitleText => titleText;

    protected override void Init()
    {
    }
}

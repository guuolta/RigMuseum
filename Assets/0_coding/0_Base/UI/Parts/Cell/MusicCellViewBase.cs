using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MusicCellViewBase : CaptionPanelViewBase
{
    [Header("番号テキスト")]
    [SerializeField]
    private TMP_Text _idText;
    /// <summary>
    /// 番号テキスト
    /// </summary>
    public TMP_Text IDText => _idText;
    [Header("製作者テキスト")]
    [SerializeField]
    private TMP_Text _authorText;
    /// <summary>
    /// 製作者テキスト
    /// </summary>
    public TMP_Text AuthorText => _authorText;
    [Header("再生時間テキスト")]
    [SerializeField]
    private TMP_Text _playTimeText;
    /// <summary>
    /// 再生時間テキスト
    /// </summary>
    public TMP_Text PlayTimeText => _playTimeText;
    [Header("お気に入りボタン")]
    [SerializeField]
    private ButtonBase _favoriteButton;
    /// <summary>
    /// お気に入りボタン
    /// </summary>
    public ButtonBase FavoriteButton => _favoriteButton;
    [Header("キャプションボタン")]
    [SerializeField]
    private ButtonBase _captionButton;
    /// <summary>
    /// キャプションボタン
    /// </summary>
    public ButtonBase CaptionButton => _captionButton;
    [Header("説明テキストの親オブジェクト")]
    [SerializeField]
    private RectTransform _explainParent;

    private Image _image;

    protected override void Init()
    {
        base.Init();
        _image = GetComponent<Image>();
        Hide(_image);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Show(_image);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        Hide(_image);
    }

    public void DownExplainText()
    {
        _explainParent.anchoredPosition = new Vector2(_explainParent.anchoredPosition.x, -_explainParent.anchoredPosition.y);
    }
}

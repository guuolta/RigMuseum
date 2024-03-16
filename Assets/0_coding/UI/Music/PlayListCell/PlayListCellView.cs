using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayListCellView : CaptionPanelViewBase
{
    [Header("背景画像")]
    [SerializeField]
    private Image _backImage;
    /// <summary>
    /// 背景画像
    /// </summary>
    public Image BackImage => _backImage;
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
    [Header("キャプションパネル")]
    [SerializeField]
    private MusicCaptionPanelPresenter _captionPanel;
    /// <summary>
    /// キャプションパネル
    /// </summary>
    public MusicCaptionPanelPresenter CaptionPanel => _captionPanel;

    protected override void Init()
    {
        base.Init();
        ChangeInteractive(false);
    }
}

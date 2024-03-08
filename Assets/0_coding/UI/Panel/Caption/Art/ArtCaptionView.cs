using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArtCaptionView : CaptionPanelViewBase
{
    [Header("製作者テキスト")]
    [SerializeField]
    private TMP_Text _authorText;
    /// <summary>
    /// 製作者テキスト
    /// </summary>
    public TMP_Text AuthorText => _authorText;
}

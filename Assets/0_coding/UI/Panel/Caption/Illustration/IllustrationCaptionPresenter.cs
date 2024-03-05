using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllustrationCaptionPresenter : CaptionPanelPresenterBase<IllustrationCaptionView>
{
    /// <summary>
    /// 製作者を設定
    /// </summary>
    /// <param name="authors"> 製作者 </param>
    public void SetAuthorText(string[] authors)
    {
        SetMemberText(authors, View.AuthorText);
    }
}

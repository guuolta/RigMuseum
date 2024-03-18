using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MusicCaptionPanelPresenter : CaptionPanelPresenterBase<MusicCaptionPanelView>
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

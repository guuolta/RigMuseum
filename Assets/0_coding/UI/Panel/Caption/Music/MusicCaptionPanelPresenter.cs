using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MusicCaptionPanelPresenter : CaptionPanelPresenterBase<MusicCaptionPanelView>
{

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventMaskImage(Ct);
    }

    /// <summary>
    /// 製作者を設定
    /// </summary>
    /// <param name="authors"> 製作者 </param>
    public void SetAuthorText(string[] authors)
    {
        SetMemberText(authors, View.AuthorText);
    }

    private void SetEventMaskImage(CancellationToken ct)
    {
        View.Mask.OnClickCallback += async () =>
        {
            await View.HideAsync(ct);
        };
    }
}

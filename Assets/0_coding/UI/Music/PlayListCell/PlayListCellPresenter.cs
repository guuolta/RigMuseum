using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayListCellPresenter : CaptionPanelPresenterBase<PlayListCellView>
{
    /// <summary>
    /// 製作者を設定
    /// </summary>
    /// <param name="authors"> 製作者 </param>
    public void SetAuthorText(string[] authors)
    {
        SetMemberText(authors, View.AuthorText);
    }

    /// <summary>
    /// 再生時間を設定
    /// </summary>
    /// <param name="time"> 再生時間 </param>
    public void SetPlayTimeText(int time)
    {
        View.PlayTimeText.text = BGMManager.Instance.GetVideoTime(time);
    }

    public void SetEventFavoriteButton()
    {
        View.FavoriteButton.OnClickCallback += () =>
        {
            // お気に入りボタン押下時の処理
        };
    }

    public void SetEventCaptionButton(CancellationToken ct)
    {
        View.CaptionButton.OnClickCallback += async () =>
        {
            await View.CaptionPanel.ShowAsync(ct);
        };
    }
}

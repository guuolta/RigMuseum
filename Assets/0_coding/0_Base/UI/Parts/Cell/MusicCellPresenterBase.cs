using System.Threading;

public class MusicCellPresenterBase<TView> : CaptionPanelPresenterBase<TView>
    where TView : MusicCellViewBase
{
    private int _id = -1;
    /// <summary>
    /// 曲番号
    /// </summary>
    protected int ID => _id;

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventClick();
        SetEventFavoriteButton();
        SetEventCaptionButton(Ct);
    }

    /// <summary>
    /// セルをクリックされたときのイベント設定
    /// </summary>
    private void SetEventClick()
    {
        View.OnClickCallback += () =>
        {
            if (_id < 0)
                return;

            PhonographMusicPlayerManager.Instance.Play(_id);
        };
    }

    /// <summary>
    /// お気に入りボタンを押されたときのイベント設定
    /// </summary>
    private void SetEventFavoriteButton()
    {
        View.FavoriteButton.OnClickCallback += () =>
        {
            // お気に入りボタン押下時の処理
        };
    }

    /// <summary>
    /// キャプションボタンを押されたときのイベント設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventCaptionButton(CancellationToken ct)
    {
        View.CaptionButton.OnClickCallback += async () =>
        {
            await PhonographMusicPlayerManager.Instance.ShowCaptionAsync(_id, ct);
        };
    }

    /// <summary>
    /// 番号を設定
    /// </summary>
    /// <param name="id"> 番号 </param>
    public void SetID(int id)
    {
        View.IDText.text = (id + 1).ToString();
        _id = id;

        if (_id == 0)
        {
            View.DownExplainText();
        }
    }

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
    public void SetPlayTimeText(string time)
    {
        View.PlayTimeText.text = time;
    }
}

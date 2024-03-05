using TMPro;

public class CaptionPanelPresenterBase<TView> : PresenterBase<TView>
    where TView : CaptionPanelViewBase
{
    /// <summary>
    /// 作品の名前を設定する
    /// </summary>
    /// <param name="title"></param>
    public void SetTitleText(string title)
    {
        View.TitleText.text = title;
    }

    /// <summary>
    /// ゲームの説明を設定する
    /// </summary>
    /// <param name="explain"> 説明 </param>
    public void SetExplain(string explain)
    {
        SetText(explain, View.ExplainText);
    }

    /// <summary>
    /// テキストを設定する
    /// </summary>
    /// <param name="content"> 内容 </param>
    /// <param name="text"> テキスト </param>
    protected void SetText(string content, TMP_Text text)
    {
        if (content == "")
        {
            View.HideText(text);
            return;
        }

        text.text = content;
    }

    /// <summary>
    /// メンバーのテキスト設定
    /// </summary>
    /// <param name="members"> メンバー </param>
    /// <param name="memberText"> テキスト </param>
    protected void SetMemberText(string[] members, TMP_Text memberText)
    {
        if (members.Length == 0)
        {
            View.HideText(memberText);
            return;
        }

        string text = "";
        int i;
        for (i = 0; i < members.Length - 1; i++)
        {
            text += members[i] + ", ";
        }
        text += members[i];

        memberText.text = text;
    }
}

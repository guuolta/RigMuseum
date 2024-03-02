using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;

public class GameCaptionPresenter : PresenterBase<GameCaptionView>
{
    private CompositeDisposable disposables = new CompositeDisposable();

    protected override void SetEvent()
    {
        SetEventShowCaption();
    }

    /// <summary>
    /// ゲームの名前を設定する
    /// </summary>
    /// <param name="title"></param>
    public void SetTitleText(string title)
    {
        View.TitleText.text = title;
    }

    /// <summary>
    /// コーディングメンバーを設定
    /// </summary>
    /// <param name="members"> メンバー </param>
    public void SetCoadingMemberText(string[] members)
    {
        SetMemberText(members, View.CoadingMemberText);
    }

    /// <summary>
    /// 2Dメンバーを設定
    /// </summary>
    /// <param name="members"> メンバー </param>
    public void SetIllustrationMemberText(string[] members)
    {
        SetMemberText(members, View.IllustrationMemberText);
    }

    /// <summary>
    /// 3Dメンバーを設定
    /// </summary>
    /// <param name="members"> メンバー </param>
    public void SetModelMemberText(string[] members)
    {
        SetMemberText(members, View.ModelMemberText);
    }

    /// <summary>
    /// DTMメンバーを設定
    /// </summary>
    /// <param name="members"> メンバー </param>
    public void SetDTMMemberText(string[] members)
    {
        SetMemberText(members, View.DtmMemberText);
    }

    /// <summary>
    /// URL設定
    /// </summary>
    /// <param name="url"> URL </param>
    public void SetURL(string url)
    {
        SetText(url, View.UrlText);
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
    private void SetText(string content, TMP_Text text)
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
    private void SetMemberText(string[] members, TMP_Text memberText)
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

    /// <summary>
    /// パネルを動かすイベントを設定するか設定
    /// </summary>
    private void SetEventShowCaption()
    {
        View.CaptionButton.IsPointerDown
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                if(value)
                {
                    SetEventMoveCaption();
                }
                else
                {
                    disposables = DisposeEvent(disposables);
                }
            });
    }

    /// <summary>
    /// パネルを動かすイベント設定
    /// </summary>
    private void SetEventMoveCaption()
    {
        disposables = DisposeEvent(disposables);

        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                Vector2 pos = Vector2.zero;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(View.CanvasRectTransform, Input.mousePosition, View.CanvasCamera, out pos);
                View.MovePosY(pos.y);
            }).AddTo(disposables);
    }
}

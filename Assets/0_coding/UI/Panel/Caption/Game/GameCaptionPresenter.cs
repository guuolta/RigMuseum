using UniRx;
using UnityEngine;

public class GameCaptionPresenter : CaptionPanelPresenterBase<GameCaptionView>
{
    private CompositeDisposable disposables = new CompositeDisposable();

    protected override void SetEvent()
    {
        SetEventShowCaption();
        SetEventClipButton();
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
    /// クリップボタンのイベント設定
    /// </summary>
    private void SetEventClipButton()
    {
        View.ClipButton.OnClickCallback += () =>
        {
            GUIUtility.systemCopyBuffer = View.UrlText.text;
        };
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

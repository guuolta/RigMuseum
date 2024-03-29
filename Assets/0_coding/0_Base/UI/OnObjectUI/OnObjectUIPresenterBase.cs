public class OnObjectUIPresenterBase<TView> : PresenterBase<TView>
    where TView : OnObjectUIViewBase
{
    protected override void SetEvent()
    {
        SetEventCloseButton();
    }

    /// <summary>
    /// 閉じるボタンのイベント設定
    /// </summary>
    protected virtual void SetEventCloseButton()
    {
        View.CloseButton.OnClickCallback += () =>
        {
            GameStateManager.SetMuseumState(MuseumState.Play);
        };
    }
}
public class OnObjectUIPresenterBase<TView> : PresenterBase<TView>
    where TView : OnObjectUIViewBase
{
    protected override void SetEvent()
    {
        SetEventCloseButton();
    }
    private void SetEventCloseButton()
    {
        View.CloseButton.OnClickCallback += () =>
        {
            GameStateManager.SetMuseumState(MuseumState.Play);
        };
    }
}

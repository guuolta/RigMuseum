using UniRx;

public class SoundPanelPresenter : PanelPresenterBase<SoundPanelView>
{
    protected override void SetEvent()
    {
        SetValue();
        SetEventValueUIPart();
    }

    private void SetValue()
    {
        float[] volumes = SaveManager.GetSoundVolume();

        View.MasterValueUIPart.SetValue(volumes[(int)AudioType.Master]);
        View.BGMValueUIPart.SetValue(volumes[(int)AudioType.BGM]);
        View.SEValueUIPart.SetValue(volumes[(int)AudioType.SE]);
        View.MovieValueUIPart.SetValue(volumes[(int)AudioType.Movie]);
    }

    private void SetEventValueUIPart()
    {
        View.MasterValueUIPart.Value
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                AudioManager.Instance.SetMasterVolume(value);
            });

        View.BGMValueUIPart.Value
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                AudioManager.Instance.SetBGMVolume(value);
            }).AddTo(this);

        View.SEValueUIPart.Value
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                AudioManager.Instance.SetSEVolume(value);
            }).AddTo(this);

        View.MovieValueUIPart.Value
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                AudioManager.Instance.SetMovieVolume(value);
            }).AddTo(this);
    }
}

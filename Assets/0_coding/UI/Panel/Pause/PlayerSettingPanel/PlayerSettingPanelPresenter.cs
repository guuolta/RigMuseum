using Cysharp.Threading.Tasks;
using UniRx;

public class PlayerSettingPanelPresenter : PanelPresenterBase<PlayerSettingPanelView>
{
    public override void Init()
    {
        SetValue();
    }

    public override void SetEvent()
    {
        base.SetEvent();
        SetEventValue();
        SetEventToggle();
    }

    private void SetValue()
    {
        View.MoveSpeedUI.SetValue(SaveManager.GetMoveSpeed());
        View.SensitivityUI.SetValue(SaveManager.GetSensitivity());
        View.VerticulToggle.SetValue(SaveManager.GetIsVerticalReverse());
        View.HorizontalToggle.SetValue(SaveManager.GetIsHorizontalReverse());
    }

    private void SetEventValue()
    {
        View.MoveSpeedUI.Value
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                PlayerManager.Instance.SetMoveSpeed(value);
            }).AddTo(this);

        View.SensitivityUI.Value
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                PlayerManager.Instance.SetSensitivity(value);
            }).AddTo(this);
    }

    private void SetEventToggle()
    {
        View.VerticulToggle.IsToggle
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                PlayerManager.Instance.SetIsReveseVertical(value);
            }).AddTo(this);

        View.HorizontalToggle.IsToggle
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                PlayerManager.Instance.SetIsReverseHorizontal(value);
            }).AddTo(this);
    }
}

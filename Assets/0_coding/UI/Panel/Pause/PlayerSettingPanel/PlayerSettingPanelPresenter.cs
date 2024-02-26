using Cysharp.Threading.Tasks;
using UniRx;

public class PlayerSettingPanelPresenter : PanelPresenterBase<PlayerSettingPanelView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetValue();
        SetEventValue();
        SetEventCheckBox();
    }

    private void SetValue()
    {
        View.MoveSpeedUI.SetValue(SaveManager.GetMoveSpeed());
        View.SensitivityUI.SetValue(SaveManager.GetSensitivity());
        View.VerticulCheckBox.SetValue(SaveManager.GetIsVerticalReverse());
        View.HorizontalCheckBox.SetValue(SaveManager.GetIsHorizontalReverse());
    }

    private void SetEventValue()
    {
        View.MoveSpeedUI.Value
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                PlayerManager.Instance.SetMoveSpeed(value);
            });

        View.SensitivityUI.Value
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                PlayerManager.Instance.SetSensitivity(value);
            });
    }

    private void SetEventCheckBox()
    {
        View.VerticulCheckBox.IsCheck
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                PlayerManager.Instance.SetIsReveseVertical(value);
            });

        View.HorizontalCheckBox.IsCheck
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                PlayerManager.Instance.SetIsReverseHorizontal(value);
            });
    }
}

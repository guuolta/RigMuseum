using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MousePanelPresenter : PanelPresenterBase<MousePanelView>
{
    public override void SetEvent()
    {
        base.SetEvent();
        SetEventSensitivity();
        SetEventToggle();
    }

    private void SetEventSensitivity()
    {
        View.SensitivityUI.Value
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                PlayerOperater.Instance.SetSensitivity(value);
            }).AddTo(this);
    }

    private void SetEventToggle()
    {
        View.VirticulToggle.IsToggle
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                PlayerOperater.Instance.SetIsReveseVertical(value);
            }).AddTo(this);

        View.HorizontalToggle.IsToggle
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                PlayerOperater.Instance.SetIsReverseHorizontal(value);
            }).AddTo(this);
    }
}

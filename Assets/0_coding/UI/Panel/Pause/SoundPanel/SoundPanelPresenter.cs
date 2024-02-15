using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class SoundPanelPresenter : PanelPresenterBase<SoundPanelView>
{
    public override void SetEvent()
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
    }

    private void SetEventValueUIPart()
    {
        View.MasterValueUIPart.Value
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                AudioManager.Instance.SetMasterVolume(value);
            }).AddTo(this);

        View.BGMValueUIPart.Value
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                AudioManager.Instance.SetBGMVolume(value);
            }).AddTo(this);

        View.SEValueUIPart.Value
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                AudioManager.Instance.SetSEVolume(value);
            }).AddTo(this);
    }
}

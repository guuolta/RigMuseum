using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterVolumeSliderPresenter : VolumeSliderPresenter
{
    public override float GetVolume()
    {
        return SaveManager.Instance.GetMasterVolume();
    }

    public override void SetVolume(float volume)
    {
        AudioManager.Instance.SetMasterVolume(volume);
    }
}

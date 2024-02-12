using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEVolumeSliderPresenter : VolumeSliderPresenter
{
    public override float GetVolume()
    {
        return SaveManager.Instance.GetSEVolume();
    }

    public override void SetVolume(float volume)
    {
        AudioManager.Instance.SetSEVolume(volume);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMVolumeSliderPresenter : VolumeSliderPresenter
{
    public override float GetVolume()
    {
        return SaveManager.Instance.GetBGMVolume();
    }

    public override void SetVolume(float volume)
    {
        AudioManager.Instance.SetBGMVolume(volume);
    }
}

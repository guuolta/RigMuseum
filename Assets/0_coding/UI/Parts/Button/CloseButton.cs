using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseButton : ButtonBase
{
    protected override void SetEventPlaySe()
    {
        OnClickCallback += () =>
        {
            AudioManager.Instance.PlayOneShotSE(SEType.Nega);
        };
    }
}

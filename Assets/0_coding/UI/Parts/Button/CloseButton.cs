using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseButton : ButtonBase
{
    public override void SetEventPlaySe()
    {
        OnClickCallback += () =>
        {
            AudioManager.Instance.PlayOneShotSE(SEType.Nega);
        };
    }
}

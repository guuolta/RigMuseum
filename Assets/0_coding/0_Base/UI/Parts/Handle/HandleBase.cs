using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandleBase : UIAnimationPartBase
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        AudioManager.Instance.PlayOneShotSE(SEType.Posi);
    }
}

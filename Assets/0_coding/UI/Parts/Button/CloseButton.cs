using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseButton : ButtonBase
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlayOneShotSE(SEType.Nega);
        onClickCallback?.Invoke();
    }
}

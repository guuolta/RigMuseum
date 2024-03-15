using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phonograph : TouchObjectBase
{
    public override void StartTouchEvent()
    {
        GameStateManager.SetMuseumState(MuseumState.Music);
        base.StartTouchEvent();
    }
}
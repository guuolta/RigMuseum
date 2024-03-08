using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitor : TouchObjectBase
{
    public override void StartEvent()
    {
        base.StartEvent();
        GameStateManager.SetMuseumState(MuseumState.Monitor);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monitor : TouchObjectBase
{
    public override void StartEvent()
    {
        GameStateManager.SetMuseumState(MuseumState.Monitor);
    }
}

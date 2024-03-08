using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CaptionManager : ProductionManagerBase<CaptionManager>
{
    public void SetState()
    {
        GameStateManager.SetMuseumState(MuseumState.Target);
    }
}
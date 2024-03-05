using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CaptionManager : ProductionManagerBase<CaptionManager>
{
    public async UniTask Target(Transform transform, CancellationToken ct)
    {
        await PlayerManager.Instance.TargetObjectAsync(animationTime,
            GetCameraPos(transform.position, -transform.forward, distance),
            GetCameraRot(transform.localEulerAngles, new Vector3(0, 90, 0)),
            ct);
    }

    public async UniTask ClearTarget(Transform transform, CancellationToken ct)
    {
        await PlayerManager.Instance.TargetObjectAsync(animationTime,
            GetCameraPos(transform.position, -transform.forward, distance),
            GetCameraRot(transform.localEulerAngles, new Vector3(0, 90, 0)),
            ct);
    }
}
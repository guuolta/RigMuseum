using UnityEngine;

public class ArtObjectBase : TouchObjectBase
{
    [Header("カメラとオブジェクトの距離(負の値の時、初期値)")]
    [SerializeField]
    private float _distance = -1f;

    public override async void StartEvent()
    {
        base.StartEvent();
        CaptionManager.Instance.SetState();

        if(_distance < 0)
        {
            await CaptionManager.Instance.TargetAsync(this, Ct);
        }
        else
        {
            await CaptionManager.Instance.TargetAsync(this, _distance, Ct);
        }
    }
}
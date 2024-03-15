using UnityEngine;

public class ArtObjectBase : TouchObjectBase
{
    [Header("カメラとオブジェクトの距離(負の値の時、初期値)")]
    [SerializeField]
    private float _distance = -1f;

    public override async void StartTouchEvent()
    {
        base.StartTouchEvent();
        ArtManager.Instance.SetState();

        if(_distance < 0)
        {
            await ArtManager.Instance.TargetAsync(this, Ct);
        }
        else
        {
            await ArtManager.Instance.TargetAsync(this, _distance, Ct);
        }
    }
}
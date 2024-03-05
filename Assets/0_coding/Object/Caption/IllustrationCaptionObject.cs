using UnityEngine;

public class IllustrationCaptionObject : TouchObjectBase
{
    [Header("キャプションUI")]
    [SerializeField]
    private IllustrationCaptionPresenter _captionUI;
    /// <summary>
    /// キャプションUI
    /// </summary>
    public IllustrationCaptionPresenter CaptionUI => _captionUI;

    protected override void Init()
    {
        Transform.GetChild(0).GetComponentInChildren<Canvas>().worldCamera = Camera.main;
    }

    public override void StartEvent()
    {
        GameStateManager.SetMuseumState(MuseumState.Target);
    }
}

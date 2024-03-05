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

    public override async void StartEvent()
    {
        GameStateManager.SetMuseumState(MuseumState.Target);
    }
}

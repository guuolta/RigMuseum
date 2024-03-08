using UnityEngine;

public class ArtCaptionObject : ArtObjectBase
{
    [Header("キャプションUI")]
    [SerializeField]
    private ArtCaptionPresenter _captionUI;
    /// <summary>
    /// キャプションUI
    /// </summary>
    public ArtCaptionPresenter CaptionUI => _captionUI;

    protected override void Init()
    {
        Transform.GetChild(0).GetComponentInChildren<Canvas>().worldCamera = Camera.main;
    }
}

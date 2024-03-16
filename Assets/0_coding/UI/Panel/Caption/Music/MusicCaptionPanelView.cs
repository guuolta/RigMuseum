using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicCaptionPanelView : CaptionPanelViewBase
{
    [Header("マスク画像")]
    [SerializeField]
    private UIBase _mask;
    /// <summary>
    /// マスク画像
    /// </summary>
    public UIBase Mask => _mask;
    [Header("製作者テキスト")]
    [SerializeField]
    private TMP_Text _authorText;
    /// <summary>
    /// 製作者テキスト
    /// </summary>
    public TMP_Text AuthorText => _authorText;

    private Image _maskImage;

    protected override void Init()
    {
        _maskImage = _mask.GetComponent<Image>();
        base.Init();
        ChangeInteractive(false);
    }

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        Show(_maskImage);
        await base.ShowAsync(ct);
    }

    public override async UniTask HideAsync(CancellationToken ct)
    {
        Hide(_maskImage);
        await base.HideAsync(ct);
    }
}
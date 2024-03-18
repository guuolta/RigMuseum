using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicCaptionPanelView : CaptionPanelViewBase
{
    [Header("製作者テキスト")]
    [SerializeField]
    private TMP_Text _authorText;
    /// <summary>
    /// 製作者テキスト
    /// </summary>
    public TMP_Text AuthorText => _authorText;

    protected override void Init()
    {
        base.Init();
        ChangeInteractive(false);
    }

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        await base.ShowAsync(ct);
    }

    public override async UniTask HideAsync(CancellationToken ct)
    {
        await base.HideAsync(ct);
    }
}
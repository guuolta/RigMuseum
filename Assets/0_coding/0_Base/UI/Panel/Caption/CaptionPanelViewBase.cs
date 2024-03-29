using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;

public class CaptionPanelViewBase : ViewBase
{
    [Header("タイトルテキスト")]
    [SerializeField]
    private TMP_Text _titleText;
    /// <summary>
    /// タイトルテキスト
    /// </summary>
    public TMP_Text TitleText => _titleText;
    [Header("説明テキスト")]
    [SerializeField]
    private TMP_Text _descriptionText;
    /// <summary>
    /// 説明テキスト
    /// </summary>
    public TMP_Text DescriptionText => _descriptionText;

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        await ShowAsync(CanvasGroup, ct);
    }

    public override async UniTask HideAsync(CancellationToken ct)
    {
        await HideAsync(CanvasGroup, ct);
    }

    /// <summary>
    /// テキストを消す
    /// </summary>
    /// <param name="targetText"> テキスト </param>
    public void HideText(TMP_Text targetText)
    {
        targetText.transform.parent.gameObject.SetActive(false);
    }
}
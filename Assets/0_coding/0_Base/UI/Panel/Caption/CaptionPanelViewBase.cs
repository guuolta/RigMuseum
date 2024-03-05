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
    private TMP_Text _explainText;
    /// <summary>
    /// 説明テキスト
    /// </summary>
    public TMP_Text ExplainText => _explainText;

    public override UniTask ShowAsync(CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }

    public override UniTask HideAsync(CancellationToken ct)
    {
        throw new System.NotImplementedException();
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
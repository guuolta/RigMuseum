using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MusicCellView : ViewBase
{
    [Header("背景画像")]
    [SerializeField]
    private Image _backImage;
    /// <summary>
    /// 背景画像
    /// </summary>
    public Image BackImage => _backImage;
    [Header("プレイボタン")]
    [SerializeField]
    private MusicPlayButton musicPlayButton;
    /// <summary>
    /// プレイボタン
    /// </summary>
    public MusicPlayButton MusicPlayButton => musicPlayButton;

    public override UniTask ShowAsync(CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }

    public override UniTask HideAsync(CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        ShowAsync(_backImage, Ct).Forget();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        HideAsync(_backImage, Ct).Forget();
    }
}

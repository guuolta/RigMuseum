using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class MusicCellView : ViewBase
{
    [Header("背景画像")]
    [SerializeField]
    private UIBase _backImage;
    /// <summary>
    /// 背景画像
    /// </summary>
    public UIBase BackImage => _backImage;
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

    private void SetEventBackImage()
    {
        //_backImage.
    }
}

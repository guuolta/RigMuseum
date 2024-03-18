using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MusicCellView : MusicCellViewBase
{
    [Header("プレイボタン")]
    [SerializeField]
    private MusicPlayButton _musicPlayButton;
    /// <summary>
    /// プレイボタン
    /// </summary>
    public MusicPlayButton MusicPlayButton => _musicPlayButton;

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        await _musicPlayButton.ShowAsync(ct);
    }

    public override async UniTask HideAsync(CancellationToken ct)
    {
        await _musicPlayButton.HideAsync(ct);
    }
}

using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using TMPro;
using UnityEngine;

public class VideoUIView : ViewBase
{
    [Header("動画再生ボタン")]
    [SerializeField]
    private VideoOnOffButton _playButton;
    [Header("動画10秒飛ばしボタン")]
    [SerializeField]
    private VideoButton _skipButton;
    [Header("動画10秒戻しボタン")]
    [SerializeField]
    private VideoButton _backButton;
    [Header("早送りボタン")]
    [SerializeField]
    private VideoButton _speedButton;
    [Header("次の動画ボタン")]
    [SerializeField]
    private VideoButton _nextButton;
    [Header("ミュートボタン")]
    [SerializeField]
    private VideoOnOffButton _muteButton;
    [Header("音量スライダー")]
    [SerializeField]
    private VideoSlider _volumeSlider;
    [Header("再生時間")]
    [SerializeField]
    private TMP_Text _playTimeText;
    [Header("動画の時間")]
    [SerializeField]
    private TMP_Text _videoTimeText;


    protected override void Init()
    {
        CanvasGroup.alpha = 0f;
        ChangeInteractive(false);
    }

    public override async UniTask ShowAsync(float animationTime, CancellationToken ct)
    {
        await CanvasGroup
            .DOFade(1, animationTime)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken: ct);
    }

    public override async UniTask HideAsync(float animationTime, CancellationToken ct)
    {
        await CanvasGroup
            .DOFade(0, animationTime)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken: ct);
    }
}

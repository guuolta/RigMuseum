using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

public class MusicVolumePanelView : PanelViewBase
{
    [Header("ミュートボタン")]
    [SerializeField]
    private MediaOnOffButton _muteButton;
    /// <summary>
    /// ミュートボタン
    /// </summary>
    public MediaOnOffButton MuteButton => _muteButton;
    [Header("音量UI")]
    [SerializeField]
    private ValueUIPart _volumeUIPart;
    /// <summary>
    /// 音量UI
    /// </summary>
    public ValueUIPart VolumeUIPart => _volumeUIPart;

    private float _iniPosY;
    private float _targetPosY;

    protected override void Init()
    {
        float height = RectTransform.sizeDelta.y;
        _targetPosY = RectTransform.anchoredPosition.y;
        RectTransform.anchoredPosition -= new Vector2(0, height);
        _iniPosY = RectTransform.anchoredPosition.y;
    }

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        RectTransform.DOComplete();

        await RectTransform
            .DOAnchorPosY(_targetPosY, animationTime)
            .ToUniTask(cancellationToken: ct);
    }

    public override async UniTask HideAsync(CancellationToken ct)
    {
        RectTransform.DOComplete();

        await RectTransform
            .DOAnchorPosY(_iniPosY, animationTime)
            .ToUniTask(cancellationToken: ct);
    }
}
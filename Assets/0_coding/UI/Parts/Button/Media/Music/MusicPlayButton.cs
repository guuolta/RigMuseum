using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MusicPlayButton : MediaOnOffButton
{
    [Header("レコード画像")]
    [SerializeField]
    private Image _recordImage;
    [Header("背景画像")]
    [SerializeField]
    private Image _backImage;

    protected override void Init()
    {
        base.Init();
        Hide(_backImage);
        Hide(CanvasGroup);
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetRecord();
    }

    private void SetRecord()
    {
        _recordImage.transform.DORotate(new Vector3(0, 0, 360), AnimationTime, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    protected override void Destroy()
    {
        base.Destroy();
        _recordImage.transform.DOKill();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Show(_backImage);
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        Hide(_backImage);
        base.OnPointerExit(eventData);
    }

    /// <summary>
    /// ボタン表示
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask ShowAsync(CancellationToken ct)
    {
        PlayRecordAnimation();
        await ShowAsync(CanvasGroup, ct);
        ChangeInteractive(true);
    }

    /// <summary>
    /// レコードアニメーション再生
    /// </summary>
    private void PlayRecordAnimation()
    {
        _recordImage.transform.DOPlay();
    }

    /// <summary>
    /// ボタン非表示
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask HideAsync(CancellationToken ct)
    {
        PauseRecordAnimation();
        await HideAsync(CanvasGroup, ct);
        ChangeInteractive(false);
    }

    /// <summary>
    /// レコードアニメーション一時停止
    /// </summary>
    private void PauseRecordAnimation()
    {
        _recordImage.transform.DOPause();
    }
}

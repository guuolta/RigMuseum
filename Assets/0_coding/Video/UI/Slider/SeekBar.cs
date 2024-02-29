using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

public class SeekBar : SliderBase
{
    private RectTransform _handleRect;
    private Vector2 _iniSize = Vector2.zero;
    private Vector2 _targetSize = Vector2.zero;

    protected override void Init()
    {
        _handleRect = Slider.handleRect;
        _targetSize = RectTransform.sizeDelta;
        _iniSize = _targetSize - new Vector2(0, _targetSize.y * 0.5f);

        RectTransform.sizeDelta = _iniSize;
        _handleRect.localScale = Vector2.zero;
    }

    public override async void OnPointerEnter(PointerEventData eventData)
    {
        await Show(Ct);
    }

    public override async void OnPointerExit(PointerEventData eventData)
    {
        await Hide(Ct);
    }

    public async UniTask Show(CancellationToken ct)
    {
        await RectTransform
            .DOSizeDelta(_targetSize, animationTime)
            .SetEase(Ease.Linear)
            .ToUniTask(cancellationToken : ct);
        _handleRect.localScale = Vector2.one;
    }

    public async UniTask Hide(CancellationToken ct)
    {
        _handleRect.localScale = Vector2.zero;
        await RectTransform
            .DOSizeDelta(_iniSize, animationTime)
            .SetEase(Ease.Linear)
            .ToUniTask(cancellationToken: ct);
    }
}

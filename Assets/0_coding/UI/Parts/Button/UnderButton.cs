using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnderButton : ButtonBase
{
    /// <summary>
    /// ボタンにカーソルがあった時のイベントときのイベント
    /// </summary>
    public System.Action onEnterEvent;

    [Header("カーソルがあった時のボタンの濃さ")]
    [Range(0f, 1f)]
    [SerializeField]
    private float _alpha = 0.8f;

    private BoolReactiveProperty _isPointerDown = new BoolReactiveProperty(false);
    /// <summary>
    /// ボタンを押したか
    /// </summary>
    public BoolReactiveProperty IsPointerDown => _isPointerDown;

    private float _iniAlpha;
    private CompositeDisposable disposables = new CompositeDisposable();

    protected override void Init()
    {
        base.Init();
        _iniAlpha = CanvasGroup.alpha;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown.Value = true;
        SetEventPointerUp();
        base.OnPointerDown(eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
        {
            return;
        }

        CanvasGroup.DOFade(_alpha, animationTime).SetEase(Ease.InSine);
    }

    public override async void OnPointerExit(PointerEventData eventData)
    {
        await UniTask.WaitUntil(() => !Input.GetMouseButton(0));
        base.OnPointerExit(eventData);
        CanvasGroup.DOFade(_iniAlpha, animationTime).SetEase(Ease.OutSine).ToUniTask().Forget();
    }

    private void SetEventPointerUp()
    {
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetMouseButtonUp(0))
            .Subscribe(_ =>
            {
                _isPointerDown.Value = false;
                 disposables = DisposeEvent(disposables);
            }).AddTo(disposables);
    }
}

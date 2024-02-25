using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class ButtonBase : UIBase,
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    /// <summary>
    /// ボタンが押されたときのイベント
    /// </summary>
    public System.Action onClickCallback;

    [Header("ボタンのアニメーションの時間")]
    [SerializeField]
    private float _animationTime = 0.1f;
    public float AnimationTime => _animationTime;

    public override void Init()
    {
        ChangeInteractive(true);
    }

    public override void SetEvent()
    {
        SetEventButton();
    }

    /// <summary>
    /// ボタン連打防止
    /// </summary>
    public virtual void SetEventButton()
    {
        onClickCallback += async () =>
        {
            ChangeInteractive(false);
            await UniTask.WaitForSeconds(0.1f);
            ChangeInteractive(true);
        };
    }

    /// <summary>
    /// ボタンが押されたとき処理を実行
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlayOneShotSE(SEType.Posi);
        onClickCallback?.Invoke();
    }

    /// <summary>
    /// ボタンが押し込まれたときの処理
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Transform.DOScale(0.8f, _animationTime).SetEase(Ease.InSine);
        CanvasGroup.DOFade(0.8f, _animationTime).SetEase(Ease.InSine);
    }

    /// <summary>
    /// ボタンから押し離されたときの処理
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        Transform.DOScale(1f, _animationTime).SetEase(Ease.OutSine);
        CanvasGroup.DOFade(1f, _animationTime).SetEase(Ease.OutSine);
    }

    /// <summary>
    /// ボタンにカーソルが入った時の処理
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if(Input.GetMouseButton(0))
        {
            return;
        }
        Transform.DOScale(1.2f, _animationTime).SetEase(Ease.InSine);
    }

    /// <summary>
    /// ボタンからカーソルが離れた時の処理
    /// </summary>
    /// <param name="eventData"></param>
    public virtual async void OnPointerExit(PointerEventData eventData)
    {
        await UniTask.WaitUntil(() => !Input.GetMouseButton(0));

        Transform.DOScale(1f, _animationTime).SetEase(Ease.OutSine);
    }
}

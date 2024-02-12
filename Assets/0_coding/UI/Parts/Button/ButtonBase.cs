using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;


public class ButtonBase : MonoBehaviour,
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
    private CanvasGroup _buttonCanvasGroup;
    public CanvasGroup ButtonCanvasGroup => _buttonCanvasGroup;

    private void Awake()
    {
        _buttonCanvasGroup = GetComponent<CanvasGroup>();
        if (ButtonCanvasGroup == null)
        {
            _buttonCanvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        ChangeInteractive(true);
    }

    public virtual void Start()
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
        onClickCallback?.Invoke();
    }

    /// <summary>
    /// ボタンが押し込まれたときの処理
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(0.8f, _animationTime).SetEase(Ease.OutCubic);
        ButtonCanvasGroup.DOFade(0.8f, _animationTime).SetEase(Ease.OutCubic);
    }

    /// <summary>
    /// ボタンから押し離されたときの処理
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(1f, _animationTime).SetEase(Ease.OutCubic);
        ButtonCanvasGroup.DOFade(1f, _animationTime).SetEase(Ease.OutCubic);
    }

    /// <summary>
    /// ボタンにカーソルが入った時の処理
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.2f, _animationTime).SetEase(Ease.OutCubic);
    }

    /// <summary>
    /// ボタンからカーソルが離れた時の処理
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f, _animationTime).SetEase(Ease.OutCubic);
    }

    /// <summary>
    /// ボタンを押せるようにするか設定
    /// </summary>
    /// <param name="interactable">押せるか</param>
    public void ChangeInteractive(bool isInteractive)
    {
        if (_buttonCanvasGroup == null)
        {
            return;
        }

        _buttonCanvasGroup.interactable = isInteractive;
        _buttonCanvasGroup.blocksRaycasts = isInteractive;
        if (isInteractive)
        {
            _buttonCanvasGroup.alpha = 1f;
        }
        else
        {
            _buttonCanvasGroup.alpha = 0.8f;
        }
    }
}

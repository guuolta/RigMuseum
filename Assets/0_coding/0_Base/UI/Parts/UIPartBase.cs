using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// UIパーツのベース
/// </summary>
public class UIPartBase : UIBase
{
    [Header("UIパーツのアニメーション時間")]
    [SerializeField]
    private float _animationTime = 0.1f;
    /// <summary>
    /// UIパーツのアニメーションの時間
    /// </summary>
    public float AnimationTime => _animationTime;

    /// <summary>
    /// マウスポインターの設定
    /// </summary>
    /// <param name="image"> アニメーションする画像 </param>
    public virtual void SetEventPointer(Image image)
    {
        SetEventOnClick(image);
        SetEventOnPointerDown(image);
        SetEventOnPointerUp(image);
        SetEventOnPointerEnter(image);
        SetEventOnPointerExit(image);
    }

    /// <summary>
    /// 画像をクリックしたときの処理 
    /// </summary>
    /// <param name="image"></param>
    public virtual void SetEventOnClick(Image image)
    {

    }

    /// <summary>
    /// UI上でマウスを押したときの処理
    /// </summary>
    /// <param name="image"> アニメーションする画像 </param>
    public virtual void SetEventOnPointerDown(Image image)
    {
        image.OnPointerDownAsObservable()
            .Subscribe(_ =>
            {
                image.transform.DOScale(0.8f, _animationTime).SetEase(Ease.OutCubic);
                image.DOFade(0.8f, _animationTime).SetEase(Ease.OutCubic);
            }).AddTo(this);
    }

    /// <summary>
    /// UI上でマウスを離したときの処理
    /// </summary>
    /// <param name="image"> アニメーションする画像 </param>
    public virtual void SetEventOnPointerUp(Image image)
    {
        image.OnPointerUpAsObservable()
            .Subscribe(_ =>
            {
                image.transform.DOScale(1f, _animationTime).SetEase(Ease.OutCubic);
                image.DOFade(1f, _animationTime).SetEase(Ease.OutCubic);
            }).AddTo(this);
    }

    /// <summary>
    /// UI上にマウスポインターが入ったときの処理
    /// </summary>
    /// <param name="image"> アニメーションする画像 </param>
    public virtual void SetEventOnPointerEnter(Image image)
    {
        image.OnPointerEnterAsObservable()
            .Subscribe(_ =>
            {
                image.transform.DOScale(1.1f, _animationTime).SetEase(Ease.OutCubic);
            }).AddTo(this);
    }

    /// <summary>
    /// UI上からマウスポインターが離れたときの処理
    /// </summary>
    /// <param name="image"> アニメーションする画像 </param>
    public virtual void SetEventOnPointerExit(Image image)
    {
        image.OnPointerExitAsObservable()
            .Subscribe(_ =>
            {
                image.transform.DOScale(1f, _animationTime).SetEase(Ease.OutCubic);
            }).AddTo(this);
    }
}

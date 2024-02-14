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
    /// マウスポインターの設定
    /// </summary>
    /// <param name="image"> アニメーションする画像 </param>
    public virtual void SetEventPointer(Image image)
    {
        /*UI上でマウスを押したときの処理*/
        image.OnPointerDownAsObservable()
            .Subscribe(_ =>
            {
                image.transform.DOScale(0.8f, _animationTime).SetEase(Ease.OutCubic);
                image.DOFade(0.8f, _animationTime).SetEase(Ease.OutCubic);
            }).AddTo(this);

        /*UI上でマウスを離したときの処理*/
        image.OnPointerUpAsObservable()
            .Subscribe(_ =>
            {
                image.transform.DOScale(1f, _animationTime).SetEase(Ease.OutCubic);
                image.DOFade(1f, _animationTime).SetEase(Ease.OutCubic);
            }).AddTo(this);

        /*UI上にマウスポインターが入ったときの処理*/
        image.OnPointerEnterAsObservable()
            .Subscribe(_ =>
            {
                image.transform.DOScale(1.1f, _animationTime).SetEase(Ease.OutCubic);
            }).AddTo(this);

        /*UI上からマウスポインターが離れたときの処理*/
        image.OnPointerExitAsObservable()
            .Subscribe(_ =>
            {
                image.transform.DOScale(1f, _animationTime).SetEase(Ease.OutCubic);
            }).AddTo(this);
    }
}

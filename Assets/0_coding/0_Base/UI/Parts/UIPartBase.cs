using UnityEngine.EventSystems;


/// <summary>
/// UIパーツのベース
/// </summary>
public class UIPartBase : UIBase,
    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    /// <summary>
    /// UIパーツが押されたとき処理を実行
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        
    }

    /// <summary>
    /// UIパーツが押し込まれたときの処理
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        
    }

    /// <summary>
    /// UIパーツから押し離されたときの処理
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerUp(PointerEventData eventData)
    {
        
    }

    /// <summary>
    /// UIパーツにカーソルが入った時の処理
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    /// <summary>
    /// UIパーツからカーソルが離れた時の処理
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        
    }
}

///// <summary>
///// マウスポインターの設定
///// </summary>
///// <param name="image"> アニメーションする画像 </param>
//public virtual void SetEventPointer(Image image)
//{
//    SetEventOnClick(image);
//    SetEventOnPointerDown(image);
//    SetEventOnPointerUp(image);
//    SetEventOnPointerEnter(image);
//    SetEventOnPointerExit(image);
//}

///// <summary>
///// 画像をクリックしたときの処理 
///// </summary>
///// <param name="image"></param>
//public virtual void SetEventOnClick(Image image)
//{

//}

///// <summary>
///// UI上でマウスを押したときの処理
///// </summary>
///// <param name="image"> アニメーションする画像 </param>
//public virtual void SetEventOnPointerDown(Image image)
//{
//    image.OnPointerDownAsObservable()
//        .Subscribe(_ =>
//        {
//            image.transform.DOScale(0.8f, AnimationTime).SetEase(Ease.OutCubic);
//            image.DOFade(0.8f, AnimationTime).SetEase(Ease.OutCubic);
//        }).AddTo(this);
//}

///// <summary>
///// UI上でマウスを離したときの処理
///// </summary>
///// <param name="image"> アニメーションする画像 </param>
//public virtual void SetEventOnPointerUp(Image image)
//{
//    image.OnPointerUpAsObservable()
//        .Subscribe(_ =>
//        {
//            image.transform.DOScale(1f, AnimationTime).SetEase(Ease.OutCubic);
//            image.DOFade(1f, AnimationTime).SetEase(Ease.OutCubic);
//        }).AddTo(this);
//}

///// <summary>
///// UI上にマウスポインターが入ったときの処理
///// </summary>
///// <param name="image"> アニメーションする画像 </param>
//public virtual void SetEventOnPointerEnter(Image image)
//{
//    image.OnPointerEnterAsObservable()
//        .Subscribe(_ =>
//        {
//            image.transform.DOScale(1.1f, AnimationTime).SetEase(Ease.OutCubic);
//        }).AddTo(this);
//}

///// <summary>
///// UI上からマウスポインターが離れたときの処理
///// </summary>
///// <param name="image"> アニメーションする画像 </param>
//public virtual void SetEventOnPointerExit(Image image)
//{
//    image.OnPointerExitAsObservable()
//        .Subscribe(_ =>
//        {
//            image.transform.DOScale(1f, AnimationTime).SetEase(Ease.OutCubic);
//        }).AddTo(this);
//}
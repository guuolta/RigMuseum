using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// パネルのビューのベース
/// </summary>
public class PanelViewBase : ViewBase
{
    public override void Init()
    {
        Transform.localScale = Vector3.zero;
    }

    /// <summary>
    /// パネルを表示
    /// </summary>
    /// <param name="animeTime"> アニメーションの時間 </param>
    /// <returns></returns>
    public override async UniTask ShowAsync(float animeTime)
    {
        if(Transform.localScale != Vector3.zero)
        {
            return;
        }

        await Transform.DOScale(Vector2.one, animeTime)
            .SetEase(Ease.InSine)
            .AsyncWaitForCompletion();
    }
    
    /// <summary>
    /// パネルを隠す
    /// </summary>
    /// <param name="animeTime"> アニメーションの時間 </param>
    /// <returns></returns>
    public override async UniTask HideAsync(float animeTime)
    {
        if(Transform.localScale == Vector3.zero)
        {
            return;
        }

        await Transform.DOScale(Vector2.zero, animeTime)
            .SetEase(Ease.OutSine)
            .AsyncWaitForCompletion();
    }
}

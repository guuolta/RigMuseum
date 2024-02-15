using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// パネルのビューのベース
/// </summary>
public class PanelViewBase : UIBase
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
    public async UniTask ShowPanelAsync(float animeTime)
    {
        if(Transform.localScale != Vector3.zero)
        {
            return;
        }

        await Transform.DOScale(Vector2.one, animeTime).AsyncWaitForCompletion();
    }
    
    /// <summary>
    /// パネルを隠す
    /// </summary>
    /// <param name="animeTime"> アニメーションの時間 </param>
    /// <returns></returns>
    public async UniTask ClosePanelAsync(float animeTime)
    {
        if(Transform.localScale == Vector3.zero)
        {
            return;
        }

        await Transform.DOScale(Vector2.zero, animeTime).AsyncWaitForCompletion();
    }
}

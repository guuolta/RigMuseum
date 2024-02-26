using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class ButtonBase : UIAnimationPartBase
{
    /// <summary>
    /// ボタンが押されたときのイベント
    /// </summary>
    public System.Action onClickCallback;

    protected override void Init()
    {
        ChangeInteractive(true);
    }

    protected override void SetEvent()
    {
        SetEventPlaySe();
        SetEventDobleClickPrevention();
    }

    /// <summary>
    /// SEを鳴らす
    /// </summary>
    public virtual void SetEventPlaySe()
    {
        onClickCallback += () =>
        {
            AudioManager.Instance.PlayOneShotSE(SEType.Posi);
        };
    }

    /// <summary>
    /// 連打防止
    /// </summary>
    public virtual void SetEventDobleClickPrevention()
    {
        onClickCallback += async () =>
        {
            ChangeInteractive(false);
            await UniTask.WaitForSeconds(0.1f);
            ChangeInteractive(true);
        };
    }

    /// <summary>
    /// ボタンが押されたときイベントを実行
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerClick(PointerEventData eventData)
    {
        onClickCallback?.Invoke();
    }
}

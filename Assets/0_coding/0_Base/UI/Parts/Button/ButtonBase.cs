using Cysharp.Threading.Tasks;


public class ButtonBase : UIAnimationPartBase
{
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
    protected virtual void SetEventPlaySe()
    {
        OnClickCallback += () =>
        {
            AudioManager.Instance.PlayOneShotSE(SEType.Posi);
        };
    }

    /// <summary>
    /// 連打防止
    /// </summary>
    protected virtual void SetEventDobleClickPrevention()
    {
        OnClickCallback += async () =>
        {
            ChangeInteractive(false);
            await UniTask.WaitForSeconds(0.1f, cancellationToken: Ct);
            ChangeInteractive(true);
        };
    }
}

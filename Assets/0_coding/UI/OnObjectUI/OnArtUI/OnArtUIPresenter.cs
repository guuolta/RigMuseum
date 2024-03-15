using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UnityEngine;

public class OnArtUIPresenter : OnObjectUIPresenterBase<OnArtUIView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventShow(Ct);
    }

    protected override void SetEventCloseButton()
    {
        base.SetEventCloseButton();
        View.CloseButton.OnClickCallback += async () =>
        {
            await ArtManager.Instance.ClearTargetAsync(Ct);
        };
    }

    /// <summary>
    /// UIを表示するイベント設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventShow(CancellationToken ct)
    {
        GameStateManager.MuseumStatus
            .TakeUntilDestroy(this)
            .Select(value => value == MuseumState.Target)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                if(value)
                {
                    ShowAsync(ct).Forget();
                }
                else
                {
                    HideAsync(ct).Forget();
                }
            });
    }
}

using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;

public class OnArtUIPresenter : OnObjectUIPresenterBase<OnArtUIView>
{
    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventShow(Ct);
    }

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

using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;

public class SpeedPanelPresenter : PresenterBase<SpeedPanelView>
{
    private bool _isOpen;
    /// <summary>
    /// パネルを開いているか
    /// </summary>
    public bool IsOpen => _isOpen;
    private ReactiveProperty<int> _onIndex = new ReactiveProperty<int>(0);
    /// <summary>
    /// 選択されているセル番号
    /// </summary>
    public ReactiveProperty<int> OnIndex => _onIndex;

    protected override void SetEvent()
    {
        SetEventCell(Ct);
    }

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        await base.ShowAsync(ct);
        _isOpen = true;
    }

    public override async UniTask HideAsync(CancellationToken ct)
    {
        _isOpen = false;
        await base.HideAsync(ct);
    }

    private void SetEventCell(CancellationToken ct)
    {
        View.SpeedCells[0].OnClickCallback += async () =>
        {
            await HideAsync(ct);
        };

        for(int i=1;  i<View.SpeedCells.Length; i++)
        {
            int index = i;
            View.SpeedCells[i].OnClickCallback += () =>
            {
                _onIndex.Value = index;
            };
        }

        OnIndex
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                for (int i = 1; i < View.SpeedCells.Length; i++)
                {
                    if (i != value)
                    {
                        View.SpeedCells[i].SetCheck(false);
                    }
                }
            });

        View.SpeedCells[4].SetCheck(true);
    }
}

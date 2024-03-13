using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;

public class MediaOnOffButton : MediaButton
{
    private BoolReactiveProperty _isOn = new BoolReactiveProperty(false);
    /// <summary>
    /// ボタンがONの状態か
    /// </summary>
    public BoolReactiveProperty IsOn => _isOn;

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventIsOn(Ct);
    }

    /// <summary>
    /// ボタンのOn、Off設定
    /// </summary>
    /// <param name="isOn">Onか</param>
    public void SetOn(bool isOn)
    {
        _isOn.Value = isOn;
    }

    /// <summary>
    /// ボタンが押されたときのイベント設定
    /// </summary>
    protected override void SetEventClick(CancellationToken ct)
    {
        OnClickCallback += () =>
        {
            _isOn.Value = !_isOn.Value;
        };
    }

    /// <summary>
    /// ボタンがOn、Offの時のイベント
    /// </summary>
    private void SetEventIsOn(CancellationToken ct)
    {
        IsOn
            .Skip(1)
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(async _ =>
            {
                HideAsync(TargetData.ButtonImage, ct).Forget();
                TargetData.ExplainText.HideAsync(ct).Forget();
                SetNextData();
                await ShowAsync(TargetData.ButtonImage, ct);
            });
    }
}

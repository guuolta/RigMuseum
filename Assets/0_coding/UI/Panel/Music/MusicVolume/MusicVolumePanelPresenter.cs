using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;

public class MusicVolumePanelPresenter : PanelPresenterBase<MusicVolumePanelView>
{
    protected override void SetEvent()
    {
        SetValue();
        SetEventMuteButton();
        SetEventValueUIPart();
    }

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        SetValue();
        await base.ShowAsync(ct);
    }

    /// <summary>
    /// 音量の設定
    /// </summary>
    private void SetValue()
    {
        float volume = AudioManager.Instance.GetSoundVolume(AudioType.BGM);
        View.VolumeUIPart.SetValue(volume);
    }

    /// <summary>
    /// ミュートボタンのイベント設定
    /// </summary>
    private void SetEventMuteButton()
    {
        View.MuteButton.IsOn
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                AudioManager.Instance.SetMute(value, AudioType.BGM);
            });
    }

    /// <summary>
    /// 音量UIのイベント設定
    /// </summary>
    private void SetEventValueUIPart()
    {
        View.VolumeUIPart.Value
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                AudioManager.Instance.SetBGMVolume(value);
            });
    }
    
}
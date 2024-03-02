using UnityEngine;

public class PauseMenuPanelPresenter : PanelPresenterBase<PauseMenuPanelView>
{
    [Header("ポーズパネルマネージャー")]
    [SerializeField]
    private PausePanelManager _pausePanelManager;

    protected override void SetEvent()
    {
        SetEventButton();
    }

    /// <summary>
    /// ボタンのイベント
    /// </summary>
    private void SetEventButton()
    {
        View.SoundSettingButton.OnClickCallback += async () =>
        {
            await _pausePanelManager.OpenPanelAsync(PausePanelType.Sound, Ct);
        };

        View.MouseSettingButton.OnClickCallback += async () =>
        {
            await _pausePanelManager.OpenPanelAsync(PausePanelType.Mouse, Ct);
        };

        View.CreditButton.OnClickCallback += async () =>
        {
            await _pausePanelManager.OpenPanelAsync(PausePanelType.Credit, Ct);
        };
    }
}

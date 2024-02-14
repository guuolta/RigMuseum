using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuPanelPresenter : PanelPresenterBase<PauseMenuPanelView>
{
    [Header("ポーズパネルマネージャー")]
    [SerializeField]
    private PausePanelManager _pausePanelManager;

    public override void SetEvent()
    {
        SetEventButton();
    }

    /// <summary>
    /// ボタンのイベント
    /// </summary>
    private void SetEventButton()
    {
        View.SoundSettingButton.onClickCallback += async () =>
        {
            await _pausePanelManager.OpenPanelAsync(PausePanelType.Sound);
        };

        View.MouseSettingButton.onClickCallback += async () =>
        {
            await _pausePanelManager.OpenPanelAsync(PausePanelType.Mouse);
        };

        View.CreditButton.onClickCallback += async () =>
        {
            await _pausePanelManager.OpenPanelAsync(PausePanelType.Credit);
        };
    }
}

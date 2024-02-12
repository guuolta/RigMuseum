using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuPanelPresenter : PanelPresenterBase
{
    [Header("ポーズパネルマネージャー")]
    [SerializeField]
    private PausePanelManager _pausePanelManager;
    private PauseMenuPanelView _pauseMenuView;
    private void Awake()
    {
        _pauseMenuView = (PauseMenuPanelView)View;
    }

    private void Start()
    {
        SetEventButton();
    }

    private void SetEventButton()
    {
        _pauseMenuView.SoundSettingButton.onClickCallback += async () =>
        {
            await _pausePanelManager.OpenSoundSettingPanelAsync();
        };

        _pauseMenuView.MouseSettingButton.onClickCallback += async () =>
        {
            await _pausePanelManager.OpenMouseSettingPanelAsync();
        };

        _pauseMenuView.CreditButton.onClickCallback += async () =>
        {
            await _pausePanelManager.OpenCreditPanelAsync();
        };
    }

    public override async UniTask OpenPanelAsync()
    {
        await base.OpenPanelAsync();
        //_pauseMenuView.SoundSettingButton.SetInteractable(true);
        //_pauseMenuView.MouseSettingButton.SetInteractable(true);
        //_pauseMenuView.CreditButton.SetInteractable(true);
    }
}

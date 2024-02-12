using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PausePanelManager : PanelManagerBase
{
    [Header("サウンド設定パネル")]
    [SerializeField]
    private PanelPresenterBase _soundSettingPanelPresenter;
    [Header("マウス設定パネル")]
    [SerializeField]
    private PanelPresenterBase _mouseSettingPanelPresenter;
    [Header("クレジットパネル")]
    [SerializeField]
    private PanelPresenterBase _creditPanelPresenter;

    public override void Start()
    {
        base.Start();
        SetEventOpenFirstPanel();
    }

    private void SetEventOpenFirstPanel()
    {
        GameStateManager.Instance.MuseumStatus
            .Where(value => value == MuseumState.Pause)
            .Subscribe(async _ =>
            {
                await OpenFirstPanelAsync();
            }).AddTo(this);
    }

    public async UniTask OpenSoundSettingPanelAsync()
    {
        await OpenPanelAsync(_soundSettingPanelPresenter);
    }

    public async UniTask OpenMouseSettingPanelAsync()
    {
        await OpenPanelAsync(_mouseSettingPanelPresenter);
    }

    public async UniTask OpenCreditPanelAsync()
    {
        await OpenPanelAsync(_creditPanelPresenter);
    }
}

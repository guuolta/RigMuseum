using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

/// <summary>
/// ポーズパネルを管理
/// </summary>
public class PausePanelManager : UIBase
{
    private bool _isOpenMenuPanel;
    private IPresenter _activePanel;
    [Header("ポーズメニューパネル")]
    [SerializeField]
    private PauseMenuPanelPresenter _pauseMenuPanelPresenter;
    [Header("サウンド設定パネル")]
    [SerializeField]
    private SoundPanelPresenter _soundSettingPanelPresenter;
    [Header("マウス設定パネル")]
    [SerializeField]
    private PlayerSettingPanelPresenter _mouseSettingPanelPresenter;
    [Header("クレジットパネル")]
    [SerializeField]
    private CreditPanelPresenter _creditPanelPresenter;
    [Header("閉じるボタン")]
    [SerializeField]
    private CloseButton _closeButton;

    protected override void Init()
    {
        _closeButton.GameObject.SetActive(false);
    }

    protected override void SetEvent()
    {
        SetEventPanel();
        SetEventCloseButton();
    }

    /// <summary>
    /// パネルのイベント設定
    /// </summary>
    private void SetEventPanel()
    {
        GameStateManager.MuseumStatus
            .Skip(1)
            .TakeUntilDestroy(this)
            .Select(value => value == MuseumState.Pause)
            .DistinctUntilChanged()
            .Subscribe(async value =>
            {
                if(value)
                {
                    await OpenPanelAsync(PausePanelType.PauseMenu);
                }
                else
                {
                    await ClosePanelAsync();
                    AudioManager.Instance.SaveVolume();
                    PlayerManager.Instance.SaveSetting();
                }
            });
    }
     /// <summary>
     /// 閉じるボタンのイベント
     /// </summary>
    private void SetEventCloseButton()
    {
        _closeButton.onClickCallback += async () =>
        {
            if(_isOpenMenuPanel)
            {
                GameStateManager.TogglePauseState();
            }
            else
            {
                await OpenPanelAsync(PausePanelType.PauseMenu);
            }
        };
    }


    /// <summary>
    /// パネルを開く
    /// </summary>
    /// <param name="type"> 対象のパネル </param>
    /// <returns></returns>
    public async UniTask OpenPanelAsync(PausePanelType type)
    {
        await ClosePanelAsync();
        switch (type)
        {
            case PausePanelType.PauseMenu:
                _activePanel = _pauseMenuPanelPresenter;
                _isOpenMenuPanel = true;
                break;
            case PausePanelType.Sound:
                _activePanel = _soundSettingPanelPresenter;
                break;
            case PausePanelType.Mouse:
                _activePanel = _mouseSettingPanelPresenter;
                break;
            case PausePanelType.Credit:
                _activePanel = _creditPanelPresenter;
                break;
            default:
                break;
        }

        await _activePanel.ShowAsync();
        _closeButton.GameObject.SetActive(true);
    }

    /// <summary>
    /// パネルを閉じる
    /// </summary>
    /// <returns></returns>
    public async UniTask ClosePanelAsync()
    {
        if(_activePanel == null)
        {
            return;
        }

        _closeButton.GameObject.SetActive(false);
        _isOpenMenuPanel = false;
        await _activePanel.HideAsync();
    }
}

/// <summary>
/// ポーズパネルの種類
/// </summary>
public enum PausePanelType
{
    PauseMenu = 0,
    Sound = 1,
    Mouse = 2,
    Credit = 3
}

///// <summary>
///// パネルを開く
///// </summary>
///// <param name="type"> 対象のパネル </param>
///// <returns></returns>
//public async UniTask OpenPanelAsync(PausePanelType type)
//{
//    switch(type)
//    {
//        case PausePanelType.PauseMenu:
//            await ClosePanelAsync(PausePanelType.All);
//            await _pauseMenuPanelPresenter.OpenPanelAsync();
//            _isOpenMenuPanel = true;
//            break;
//        case PausePanelType.Sound:
//            await ClosePanelAsync(PausePanelType.PauseMenu);
//            await _soundSettingPanelPresenter.OpenPanelAsync();
//            break;
//        case PausePanelType.Mouse:
//            await ClosePanelAsync(PausePanelType.PauseMenu);
//            await _mouseSettingPanelPresenter.OpenPanelAsync();
//            break;
//        case PausePanelType.Credit:
//            await ClosePanelAsync(PausePanelType.PauseMenu);
//            await _creditPanelPresenter.OpenPanelAsync();
//            break;
//        default:
//            break;
//    }

//    _closeButton.GameObject.SetActive(true);
//}

///// <summary>
///// パネルを閉じる
///// </summary>
///// <param name="type"> 対象のパネル </param>
///// <returns></returns>
//private async UniTask ClosePanelAsync(PausePanelType type)
//{
//    _closeButton.GameObject.SetActive(false);
//    _isOpenMenuPanel = false;

//    switch (type)
//    {
//        case PausePanelType.PauseMenu:
//            await _pauseMenuPanelPresenter.ClosePanelAsync();
//            break;
//        case PausePanelType.Sound:
//            await _soundSettingPanelPresenter.ClosePanelAsync();
//            break;
//        case PausePanelType.Mouse:
//            await _mouseSettingPanelPresenter.ClosePanelAsync();
//            break;
//        case PausePanelType.Credit:
//            await _creditPanelPresenter.ClosePanelAsync();
//            break;
//        case PausePanelType.All:
//            await _pauseMenuPanelPresenter.ClosePanelAsync();
//            await _soundSettingPanelPresenter.ClosePanelAsync();
//            await _mouseSettingPanelPresenter.ClosePanelAsync();
//            await _creditPanelPresenter.ClosePanelAsync();
//            break;
//        default:
//            break;
//    }
//}
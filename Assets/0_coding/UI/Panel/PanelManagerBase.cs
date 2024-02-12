using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManagerBase : MonoBehaviour
{
    [Header("最初に表示するパネル")]
    [SerializeField]
    private PanelPresenterBase _firstPanel;
    private PanelPresenterBase _activePanel;
    /// <summary>
    /// 現在開いているパネル
    /// </summary>
    public PanelPresenterBase ActivePanel => _activePanel;
    [Header("閉じるボタン")]
    [SerializeField]
    private ButtonBase _closeButton;



    public virtual void Awake()
    {
        if(_firstPanel == null)
        {
            Debug.Log("最初に表示するパネルを設定してください");
        }

        _closeButton.gameObject.SetActive(false);
        _activePanel = _firstPanel;
    }

    public virtual void Start()
    {
        SetEventCloseButton();
    }

    public virtual void SetActivePanle(PanelPresenterBase panel)
    {
        _activePanel = panel;
    }


    public virtual void SetEventCloseButton()
    {
        _closeButton.onClickCallback += async () =>
        {
            Debug.Log(_activePanel.name);
            if (_activePanel == _firstPanel)
            {
                await _firstPanel.ClosePanelAsync();
                GameStateManager.Instance.SetMuseumState(MuseumState.Play);
            }
            else
            {
                await OpenPanelAsync(_firstPanel);
            }
        };
    }

    public virtual async UniTask OpenFirstPanelAsync()
    {
        await OpenPanelAsync(_firstPanel);
    }

    public virtual async UniTask OpenPanelAsync(PanelPresenterBase panel)
    {
        _closeButton.gameObject.SetActive(false);
        await _activePanel.ClosePanelAsync();
        SetActivePanle(panel);
        await panel.OpenPanelAsync();
        _closeButton.gameObject.SetActive(true);
        _closeButton.SetInteractable(true);
    }
}
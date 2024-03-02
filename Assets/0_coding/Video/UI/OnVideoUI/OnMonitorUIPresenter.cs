using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMonitorUIPresenter : PresenterBase<OnMonitorUIView>
{
    protected override void SetEvent()
    {
        SetEventCloseButton();
    }
    private void SetEventCloseButton()
    {
        View.CloseButton.OnClickCallback += () =>
        {
            GameStateManager.SetMuseumState(MuseumState.Play);
        };
    }
}

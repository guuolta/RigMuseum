using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPanelView : PanelViewBase
{
    [SerializeField]
    private ValueUIPart _masterValueUIPart;
    public ValueUIPart MasterValueUIPart => _masterValueUIPart;

    [SerializeField]
    private ValueUIPart _bgmValueUIParts;
    public ValueUIPart BGMValueUIPart => _bgmValueUIParts;

    [SerializeField]
    private ValueUIPart _seValueUIParts;
    public ValueUIPart SEValueUIPart => _seValueUIParts;
}
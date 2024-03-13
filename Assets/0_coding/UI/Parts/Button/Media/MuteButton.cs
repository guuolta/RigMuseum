using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : ButtonBase
{
    [Header("背景の画像")]
    [SerializeField]
    private Image _backImage;
    [Header("ONの画像")]
    [SerializeField]
    private Image _onImage;
    [Header("OFFの画像")]
    [SerializeField]
    private Image _offImage;
}
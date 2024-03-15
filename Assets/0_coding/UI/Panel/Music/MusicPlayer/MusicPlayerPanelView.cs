using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;

public class MusicPlayerPanelView : PanelViewBase
{
    [Header("マスク画像")]
    [SerializeField]
    private UIBase _maskImage;
    /// <summary>
    /// マスク画像
    /// </summary>
    public UIBase MaskImage => _maskImage;
    [Header("再生ボタン")]
    [SerializeField]
    private MediaOnOffButton _playButton;
    /// <summary>
    /// 再生ボタン
    /// </summary>
    public MediaOnOffButton PlayButton => _playButton;
    [Header("次の曲ボタン")]
    [SerializeField]
    private MediaButton _nextButton;
    /// <summary>
    /// 次の曲ボタン
    /// </summary>
    public MediaButton NextButton => _nextButton;
    [Header("前の曲ボタン")]
    [SerializeField]
    private MediaButton _backButton;
    /// <summary>
    /// 前の曲ボタン
    /// </summary>
    public MediaButton BackButton => _backButton;
    [Header("ループボタン")]
    [SerializeField]
    private MediaButton _loopButton;
    /// <summary>
    /// ループボタン
    /// </summary>
    public MediaButton LoopButton => _loopButton;
    [Header("シャッフルボタン")]
    [SerializeField]
    private MediaButton _shuffleButton;
    /// <summary>
    /// シャッフルボタン
    /// </summary>
    public MediaButton ShuffleButton => _shuffleButton;
    [Header("音量ボタン")]
    [SerializeField]
    private VolumeIconButton _volumeButton;
    /// <summary>
    /// 音量ボタン
    /// </summary>
    public VolumeIconButton VolumeButton => _volumeButton;
    [Header("音量パネル")]
    [SerializeField]
    private MusicVolumePanelPresenter _volumePanel;
    /// <summary>
    /// 音量パネル
    /// </summary>
    public MusicVolumePanelPresenter VolumePanel => _volumePanel;
    [Header("シークバー")]
    [SerializeField]
    private SeekBar _seekBar;
    /// <summary>
    /// シークバー
    /// </summary>
    public SeekBar SeekBar => _seekBar;
    [Header("再生時間")]
    [SerializeField]
    private TMP_Text _playTimeText;
    /// <summary>
    /// 再生時間
    /// </summary>
    public TMP_Text PlayTimeText => _playTimeText;
    [Header("残り時間")]
    [SerializeField]
    private TMP_Text _remainTimeText;
    /// <summary>
    /// 曲の時間
    /// </summary>
    public TMP_Text RemainTimeText => _remainTimeText;

    protected override void Init()
    {

    }

    public override UniTask ShowAsync(CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }

    public override UniTask HideAsync(CancellationToken ct)
    {
        throw new System.NotImplementedException();
    }
}

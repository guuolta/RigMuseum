using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoUIView : ViewBase
{
    [Header("シークバー")]
    [SerializeField]
    private SeekBar _seekBar;
    /// <summary>
    /// シークバー
    /// </summary>
    public SeekBar SeekBar => _seekBar;
    [Header("動画再生ボタン")]
    [SerializeField]
    private VideoOnOffButton _playButton;
    /// <summary>
    /// 動画再生ボタン
    /// </summary>
    public VideoOnOffButton PlayButton => _playButton; 
    [Header("動画10秒飛ばしボタン")]
    [SerializeField]
    private VideoButton _skipButton;
    /// <summary>
    /// 動画10秒飛ばしボタン
    /// </summary>
    public VideoButton SkipButton => _skipButton;
    [Header("動画10秒戻しボタン")]
    [SerializeField]
    private VideoButton _backButton;
    /// <summary>
    /// 動画10秒戻しボタン
    /// </summary>
    public VideoButton BackButton => _backButton;
    [Header("再生速度ボタン")]
    [SerializeField]
    private VideoButton _speedButton;
    /// <summary>
    /// 再生速度ボタン
    /// </summary>
    public VideoButton SpeedButton => _speedButton;
    [Header("再生速度パネル")]
    [SerializeField]
    private SpeedPanelPresenter _speedPanel;
    /// <summary>
    /// 再生速度パネル
    /// </summary>
    public SpeedPanelPresenter SpeedPanel => _speedPanel;
    [Header("次の動画ボタン")]
    [SerializeField]
    private VideoButton _nextButton;
    /// <summary>
    /// 次の動画ボタン
    /// </summary>
    public VideoButton NextButton => _nextButton;
    [Header("ミュートボタン")]
    [SerializeField]
    private VideoOnOffButton _muteButton;
    /// <summary>
    /// ミュートボタン
    /// </summary>
    public VideoOnOffButton MuteButton => _muteButton;
    [Header("音量スライダー")]
    [SerializeField]
    private VideoSlider _volumeSlider;
    /// <summary>
    /// 音量スライダー
    /// </summary>
    public VideoSlider VolumeSlider => _volumeSlider;
    [Header("再生時間")]
    [SerializeField]
    private TMP_Text _playTimeText;
    /// <summary>
    /// 再生時間
    /// </summary>
    public TMP_Text PlayTimeText => _playTimeText;
    [Header("動画の時間")]
    [SerializeField]
    private TMP_Text _videoTimeText;
    /// <summary>
    /// 動画の時間
    /// </summary>
    public TMP_Text VideoTimeText => _videoTimeText;
    [Header("自動再生トグル")]
    [SerializeField]
    private VideoToggle _toggle;
    /// <summary>
    /// 自動再生トグル
    /// </summary>
    public VideoToggle Toggle => _toggle;

    protected override void Init()
    {
        CanvasGroup.alpha = 0f;
    }

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        await ShowAsync(CanvasGroup, ct);
    }

    public override async UniTask HideAsync(CancellationToken ct)
    {
        await HideAsync(CanvasGroup, ct);
    }
}

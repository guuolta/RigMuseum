using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoUIView : ViewBase
{
    [Header("スクリーン画像")]
    [SerializeField]
    private UIBase _screenImage;
    /// <summary>
    /// スクリーン画像
    /// </summary>
    public UIBase ScreenImage => _screenImage;
    [Header("シークバー")]
    [SerializeField]
    private SeekBar _seekBar;
    /// <summary>
    /// シークバー
    /// </summary>
    public SeekBar SeekBar => _seekBar;
    [Header("動画再生ボタン")]
    [SerializeField]
    private MediaOnOffButton _playButton;
    /// <summary>
    /// 動画再生ボタン
    /// </summary>
    public MediaOnOffButton PlayButton => _playButton; 
    [Header("動画10秒飛ばしボタン")]
    [SerializeField]
    private MediaButton _skipButton;
    /// <summary>
    /// 動画10秒飛ばしボタン
    /// </summary>
    public MediaButton SkipButton => _skipButton;
    [Header("動画10秒戻しボタン")]
    [SerializeField]
    private MediaButton _backButton;
    /// <summary>
    /// 動画10秒戻しボタン
    /// </summary>
    public MediaButton BackButton => _backButton;
    [Header("再生速度ボタン")]
    [SerializeField]
    private MediaButton _speedButton;
    /// <summary>
    /// 再生速度ボタン
    /// </summary>
    public MediaButton SpeedButton => _speedButton;
    [Header("再生速度パネル")]
    [SerializeField]
    private SpeedPanelPresenter _speedPanel;
    /// <summary>
    /// 再生速度パネル
    /// </summary>
    public SpeedPanelPresenter SpeedPanel => _speedPanel;
    [Header("次の動画ボタン")]
    [SerializeField]
    private MediaButton _nextButton;
    /// <summary>
    /// 次の動画ボタン
    /// </summary>
    public MediaButton NextButton => _nextButton;
    [Header("ミュートボタン")]
    [SerializeField]
    private MediaOnOffButton _muteButton;
    /// <summary>
    /// ミュートボタン
    /// </summary>
    public MediaOnOffButton MuteButton => _muteButton;
    [Header("音量スライダー")]
    [SerializeField]
    private MediaSlider _volumeSlider;
    /// <summary>
    /// 音量スライダー
    /// </summary>
    public MediaSlider VolumeSlider => _volumeSlider;
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

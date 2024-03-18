using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using TMPro;
using UnityEngine;

public class MusicPlayerPanelView : PanelViewBase
{
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
    [Header("プレイリストボタン")]
    [SerializeField]
    private MediaOnOffButton _playlistButton;
    /// <summary>
    /// プレイリストボタン
    /// </summary>
    public MediaOnOffButton PlaylistButton => _playlistButton;
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
    [Header("プレイリストの動く距離")]
    [SerializeField]
    private float _distance;

    private float _start;
    private float _destination;

    protected override void Init()
    {
        _start = RectTransform.anchoredPosition.y;
        _destination = _start + _distance;
    }

    /// <summary>
    /// プレイリスト表示
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask ShowPlayListAsync(CancellationToken ct)
    {
        RectTransform.DOComplete();
        if(RectTransform.anchoredPosition.y == _destination)
            return;

        await RectTransform
            .DOAnchorPosY(_destination, AnimationTime)
            .SetEase(Ease.InSine)
            .ToUniTask(cancellationToken: ct);
    }

    /// <summary>
    /// プレイリスト消す
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask HidePlayListAsync(CancellationToken ct)
    {
        RectTransform.DOComplete();
        if(RectTransform.anchoredPosition.y == _start)
            return;

        await RectTransform
            .DOAnchorPosY(_start, AnimationTime)
            .SetEase(Ease.OutSine)
            .ToUniTask(cancellationToken: ct);
    }
}

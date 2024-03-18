using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using Unity.VisualScripting;

public class PhonographMusicPlayerManager : ProductionManagerBase<PhonographMusicPlayerManager>
{
    [Header("曲データ")]
    [SerializeField]
    private MusicDatas _musicDatas;
    [Header("蓄音機")]
    [SerializeField]
    private Phonograph _phonograph;
    [Header("蓄音機上のUI")]
    [SerializeField]
    private OnPhonographUIPresenter _onPhonographUI;
    [Header("ミュージックリストパネル")]
    [SerializeField]
    private MusicListPanelPresenter _musicListPanel;
    [Header("ミュージックプレイヤー")]
    [SerializeField]
    private MusicPlayerPanelPresenter _musicPlayer;
    [Header("ミュージックリストのオブジェクト")]
    [SerializeField]
    private Transform _musicListTransform;
    [Header("ミュージックセル")]
    [SerializeField]
    private MusicCellPresenter _musicCell;
    [Header("プレイリストのオブジェクト")]
    [SerializeField]
    private Transform _playListTransform;
    [Header("プレイリストセル")]
    [SerializeField]
    private PlayListCellPresenter _playListCell;
    [Header("キャプションの親オブジェクト")]
    [SerializeField]
    private Transform _captionParent;
    [Header("キャプション")]
    [SerializeField]
    private MusicCaptionPanelPresenter _captionPanel;
    [Header("マスク画像")]
    [SerializeField]
    private UIBase _mask;

    private AudioSource _recordAudioSource;
    public AudioSource RecordAudioSource
    {
        get
        {
            if(_recordAudioSource == null)
            {
                _recordAudioSource = _phonograph.GetComponent<AudioSource>();

                if(_recordAudioSource == null)
                    _recordAudioSource = _phonograph.AddComponent<AudioSource>();
            }

            return _recordAudioSource;
        }
    }
    private BoolReactiveProperty _isFinish = new BoolReactiveProperty(false);
    private BoolReactiveProperty _isPlay = new BoolReactiveProperty(false);
    /// <summary>
    /// 再生中か
    /// </summary>
    public BoolReactiveProperty IsPlay => _isPlay;
    private ReactiveProperty<int> _id = new ReactiveProperty<int>(0);
    /// <summary>
    /// 再生中の曲番号
    /// </summary>
    public ReactiveProperty<int> ID => _id;
    private ReactiveProperty<int> _playTime = new ReactiveProperty<int>(0);
    /// <summary>
    /// 再生時間
    /// </summary>
    public ReactiveProperty<int> PlayTime => _playTime;
    private ReactiveProperty<int> _length = new ReactiveProperty<int>(0);
    /// <summary>
    /// BGMの時間
    /// </summary>
    public ReactiveProperty<int> Length => _length;

    private int _dataLength;
    private IPresenter _targetPanel;
    private List<int> _playList = new List<int>();
    private List<int> _playedList = new List<int>();
    private Dictionary<int, PlayListCellPresenter> _playListDict = new Dictionary<int, PlayListCellPresenter>();
    private Dictionary<int, MusicCaptionPanelPresenter> _captionDict = new Dictionary<int, MusicCaptionPanelPresenter>();
    private CompositeDisposable _playbackDisposable = new CompositeDisposable();

    protected override void Init()
    {
        base.Init();
        _dataLength = _musicDatas.GetCount();
        _mask.ChangeInteractive(false);
        CreateMusicList();
    }

    /// <summary>
    /// 曲リスト作成
    /// </summary>
    private void CreateMusicList()
    {
        for (int i = 0; i < _dataLength; i++)
        {
            var data = _musicDatas.GetGameData(i);
            data.SetID(i);

            var musicCell = Instantiate(_musicCell, _musicListTransform);
            musicCell.SetID(data.ID);
            musicCell.SetTitleText(data.Name);
            musicCell.SetAuthorText(data.Members);
            musicCell.SetPlayTimeText(GetMusicTime((int)data.Music.length));

            var playCell = Instantiate(_playListCell, _playListTransform);
            playCell.SetID(data.ID);
            playCell.SetTitleText(data.Name);
            playCell.SetAuthorText(data.Members);
            playCell.SetPlayTimeText(GetMusicTime((int)data.Music.length));
            _playListDict.Add(data.ID, playCell);
        }
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventState(Ct);
        SetEventIsPlay();
        SetEventChangeIndex();
        SetEventPlayTime();
        SetEventFinish();
        SetEventPlay();
        SetEventMaskImage(Ct);
    }

    /// <summary>
    /// ステートによるイベント設定
    /// </summary>
    private void SetEventState(CancellationToken ct)
    {
        GameStateManager.MuseumStatus
            .TakeUntilDestroy(this)
            .Select(value => value == MuseumState.Record)
            .SkipWhile(value => !value)
            .DistinctUntilChanged()
            .Subscribe(async value =>
            {
                if (value)
                {
                    AudioManager.Instance.PauseMainBGM();
                    RecordAudioSource.spatialBlend = 0;
                    _onPhonographUI.ShowAsync(ct).Forget();
                    await TargetAsync(_phonograph, ct);

                }
                else
                {
                    if (_isPlay.Value)
                    {
                        RecordAudioSource.spatialBlend = 0;
                    }
                    else
                    {
                        AudioManager.Instance.PlayMainBGM();
                        Play();
                        RecordAudioSource.spatialBlend = 1;
                    }

                    _onPhonographUI.HideAsync(ct).Forget();
                    await ClearTargetAsync(ct);
                }
            });
    }

    /// <summary>
    /// プレイヤーの再生状態を設定
    /// </summary>
    private void SetEventIsPlay()
    {
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Select(_ => RecordAudioSource.isPlaying)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                if (value)
                {
                    _isPlay.Value = true;
                }
                else
                {
                    _isPlay.Value = false;
                }
            });
    }

    /// <summary>
    /// インデックスが変わった時のイベント設定
    /// </summary>
    private void SetEventChangeIndex()
    {
        _id
            .TakeUntilDestroy(this)
            .Where(id => 0 <= id && id < _dataLength)
            .Select(id=> _musicDatas.GetGameData(id))
            .Subscribe(value =>
            {
                _musicListPanel.SetText(value.ID+1, value.Name);
                Play(value.Music);
            });
    }

    /// <summary>
    /// 曲が終了したときのイベント設定
    /// </summary>
    private void SetEventPlay()
    {
        
    }

    /// <summary>
    /// BGMの再生時間を設定
    /// </summary>
    private void SetEventPlayTime()
    {
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Select(_ => RecordAudioSource.time)
            .DistinctUntilChanged()
            .Subscribe(time =>
            {
                _playTime.Value = (int)time;
            });
    }

    /// <summary>
    /// 再生終了のイベント設定
    /// </summary>
    private void SetEventFinish()
    {
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Select(_ => _playTime.Value >= _length.Value)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                _isFinish.Value = value;
            });
    }

    /// <summary>
    /// マスク画像のイベント設定
    /// </summary>
    /// <param name="ct"></param>
    private void SetEventMaskImage(CancellationToken ct)
    {
        _mask.OnClickCallback += async () =>
        {
            await HideOverlayPanelAsync(ct);
        };
    }

    /// <summary>
    /// 再生
    /// </summary>
    public void Play()
    {
        if (_isPlay.Value)
            return;

        RecordAudioSource.Play();
    }

    /// <summary>
    /// 再生
    /// </summary>
    /// <param name="index"> 曲番号 </param>
    public void Play(int index)
    {
        _id.SetValueAndForceNotify(index < 0 ? _dataLength -1
            : index >= _dataLength ? 0 : index);
    }

    /// <summary>
    /// 再生
    /// </summary>
    /// <param name="clip"> 曲 </param>
    public void Play(AudioClip clip)
    {
        Pause();
        SetTime(0);
        _playTime.Value = 0;
        RecordAudioSource.clip = clip;
        _length.Value = (int)clip.length;
        RecordAudioSource.Play();
    }

    private void UpdatePlayList(int id)
    {
        if (_playedList.Count == 0)
        {
            return;
        }

        while(_playedList.Count > 0)
        {
            if (_playedList[0] == id)
            {
                _playedList.RemoveAt(0);
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// 次の曲を再生
    /// </summary>
    public void PlayNext()
    {
        Play(_id.Value + 1);
    }

    /// <summary>
    /// 前の曲を再生
    /// </summary>
    public void PlayBack()
    {
        Play(_id.Value - 1);
    }

    public void PlayShuffle()
    {
        int index = Random.Range(0, _dataLength);

        foreach (int played in _playedList)
        {
            if (index == played)
            {
                PlayShuffle();
                return;
            }
        }
    }

    /// <summary>
    /// 停止
    /// </summary>
    public void Pause()
    {
        if (!_isPlay.Value)
            return;

        RecordAudioSource.Pause();
    }

    /// <summary>
    /// 音量設定
    /// </summary>
    /// <param name="volume"></param>
    public void SetVolume(float volume)
    {
        AudioManager.Instance.SetVolume(AudioType.BGM, volume);
    }

    /// <summary>
    /// ミュート設定
    /// </summary>
    /// <param name="isMute"></param>
    public void SetMute(bool isMute)
    {
        AudioManager.Instance.SetMute(isMute, AudioType.Record);
    }

    /// <summary>
    /// ループ設定
    /// </summary>
    /// <param name="isLoop"> ループするか </param>
    public void SetLoop(bool isLoop)
    {
        if (RecordAudioSource.loop == isLoop)
            return;

        RecordAudioSource.loop = isLoop;
    }

    /// <summary>
    /// 連続再生設定
    /// </summary>
    /// <param name="isSet"> 連続再生するか </param>
    public void SetPlayback(bool isSet)
    {
        _playbackDisposable = DisposeEvent(_playbackDisposable);

        if(!isSet)
        {
            return;
        }

        _isFinish
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Where(value => value)
            .Subscribe(_ =>
            {
                PlayNext();
            }).AddTo(_playbackDisposable);
    }

    /// <summary>
    /// 再生時間のテキストを取得
    /// </summary>
    /// <param name="time"> 再生時間 </param>
    /// <returns></returns>
    public string GetMusicTime(int time)
    {
        if (time < 0)
        {
            time = 0;
        }

        int minutes = time / 60;
        int seconds = time % 60;

        return $"{minutes:00}:{seconds:00}";
    }

    /// <summary>
    /// 再生時間を設定
    /// </summary>
    /// <param name="time"></param>
    public void SetTime(float time)
    {
        RecordAudioSource.time = time;
    }

    /// <summary>
    /// オーバレイUIを表示
    /// </summary>
    /// <param name="panel"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask ShowOverlayPanelAsync(IPresenter panel, CancellationToken ct)
    {
        _mask.ChangeInteractive(true);
        _targetPanel = panel;
        await _targetPanel.ShowAsync(ct);
    }

    /// <summary>
    /// オーバレイUIを消す
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask HideOverlayPanelAsync(CancellationToken ct)
    {
        await _targetPanel.HideAsync(ct);
        _mask.ChangeInteractive(false);
    }

    /// <summary>
    /// キャプションを表示
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async UniTask ShowCaptionAsync(int id, CancellationToken ct)
    {
        var caption = GetCaption(id);
        await ShowOverlayPanelAsync(caption, ct);
    }

    /// <summary>
    /// キャプション取得
    /// </summary>
    /// <param name="id"> 番号 </param>
    /// <returns></returns>
    private MusicCaptionPanelPresenter GetCaption(int id)
    {
        if(_captionDict.ContainsKey(id))
        {
            return _captionDict[id];
        }

        var caption = Instantiate(_captionPanel, _captionParent);
        var data = _musicDatas.GetGameData(id);
        caption.SetTitleText(data.Name);
        caption.SetAuthorText(data.Members);
        caption.SetDescription(data.Description);

        _captionDict.Add(id, caption);
        return caption;
    }
}

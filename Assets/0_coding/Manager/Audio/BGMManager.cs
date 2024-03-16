using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UniRx;
using UnityEngine;

public class BGMManager : ProductionManagerBase<BGMManager>
{
    [Header("曲データ")]
    [SerializeField]
    private MusicDatas _musicDatas;
    [Header("蓄音機")]
    [SerializeField]
    private Phonograph _phonograph;
    [Header("BGMのオーディオソース")]
    [SerializeField]
    private AudioSource _mainBGMAudioSource;
    [Header("通常BGM")]
    [SerializeField]
    private AudioClip _mainAudioClip;
    [Header("蓄音機上のUI")]
    [SerializeField]
    private OnPhonographUIPresenter _onPhonographUI;

    private ReactiveProperty<int> _index = new ReactiveProperty<int>(0);
    private ReactiveProperty<MusicData> _musicData = new ReactiveProperty<MusicData>();
    /// <summary>
    /// 曲データ
    /// </summary>
    public ReactiveProperty<MusicData> MusicData => _musicData;
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
    private AudioSource _recordAudioSource;
    private List<int> _playedList = new List<int>();

    protected override void Init()
    {
        base.Init();
        _dataLength = _musicDatas.GetCount();
        _recordAudioSource = _phonograph.GetComponent<AudioSource>();
        if(_phonograph == null)
        {
            Debug.LogError("蓄音機にオーディオソースが設定されていません");
            return;
        }

        PlayMainBGM();
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventState(Ct);
        SetEventPlay();
        SetEventPlayTime();
    }

    /// <summary>
    /// ステートによるイベント設定
    /// </summary>
    private void SetEventState(CancellationToken ct)
    {
        GameStateManager.MuseumStatus
            .TakeUntilDestroy(this)
            .Select(value => value == MuseumState.Music)
            .SkipWhile(value => !value)
            .DistinctUntilChanged()
            .Subscribe(async value =>
            {
                if (value)
                {
                    PauseMainBGM();
                    _recordAudioSource.spatialBlend = 0;
                    _onPhonographUI.ShowAsync(ct).Forget();
                    await TargetAsync(_phonograph, ct);

                }
                else
                {
                    if (_recordAudioSource.isPlaying)
                    {
                        _recordAudioSource.spatialBlend = 0;
                    }
                    else
                    {
                        PlayMainBGM();
                        Play();
                        _recordAudioSource.spatialBlend = 1;
                    }

                    _onPhonographUI.HideAsync(ct).Forget();
                    await ClearTargetAsync(ct);
                }
            });
    }

    /// <summary>
    /// 曲再生のイベント設定
    /// </summary>
    private void SetEventPlay()
    {
        _index
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(index =>
            {
                _musicData.Value = _musicDatas.GetGameData(index);
                Play(_musicData.Value.Music);
            });
    }

    /// <summary>
    /// BGMの再生時間を設定
    /// </summary>
    private void SetEventPlayTime()
    {
        Observable.EveryUpdate()
            .TakeUntilDestroy(this)
            .Select(_ => _recordAudioSource.time)
            .DistinctUntilChanged()
            .Subscribe(time =>
            {
                _playTime.Value = (int)time;
            });
    }

    /// <summary>
    /// 再生
    /// </summary>
    public void Play()
    {
        if (_recordAudioSource.isPlaying)
            return;

        _recordAudioSource.Play();
    }

    /// <summary>
    /// 再生
    /// </summary>
    /// <param name="index"> 曲番号 </param>
    public void Play(int index)
    {
        _index.Value = Mathf.Clamp(index, 0, _dataLength - 1);
    }

    /// <summary>
    /// 再生
    /// </summary>
    /// <param name="clip"> 曲 </param>
    public void Play(AudioClip clip)
    {
        _recordAudioSource.clip = clip;
        _length.Value = (int)clip.length;
        _recordAudioSource.Play();
    }

    /// <summary>
    /// メインBGM再生
    /// </summary>
    public void PlayMainBGM()
    {
        if(_mainBGMAudioSource.clip == null)
            _mainBGMAudioSource.clip = _mainAudioClip;
        if(_mainBGMAudioSource.isPlaying)
            return;

        _mainBGMAudioSource.Play();
    }

    /// <summary>
    /// メインBGM停止
    /// </summary>
    public void PauseMainBGM()
    {
        if(!_mainBGMAudioSource.isPlaying)
            return;

        _mainBGMAudioSource.Pause();
    }

    /// <summary>
    /// 次の曲を再生
    /// </summary>
    public void PlayNext()
    {
        if(_index.Value == _dataLength - 1)
        {
            _index.Value = 0;
            return;
        }

        _index.Value++;
    }

    /// <summary>
    /// 前の曲を再生
    /// </summary>
    public void PlayBack()
    {
        if(_index.Value == 0)
        {
            _index.Value = _dataLength - 1;
            return;
        }

        _index.Value--;
    }

    public void PlayShuffle()
    {
        int index = Random.Range(0, _dataLength);

        foreach(int played in _playedList)
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
        if (!_recordAudioSource.isPlaying)
            return;

        _recordAudioSource.Pause();
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
        if(_recordAudioSource.mute == isMute)
            return;

        _recordAudioSource.mute = isMute;
    }

    /// <summary>
    /// ループ設定
    /// </summary>
    /// <param name="isLoop"> ループするか </param>
    public void SetLoop(bool isLoop)
    {
        if (_recordAudioSource.loop == isLoop)
            return;

        _recordAudioSource.loop = isLoop;
    }

    /// <summary>
    /// 再生時間のテキストを取得
    /// </summary>
    /// <param name="time"> 再生時間 </param>
    /// <returns></returns>
    public string GetVideoTime(int time)
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
        _recordAudioSource.time = time;
    }
}

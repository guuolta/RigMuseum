using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BGMManager : ProductionManagerBase<BGMManager>
{
    [Header("BGMのオーディオソース")]
    [SerializeField]
    private AudioSource _bgmAudioSource;
    [Header("蓄音機のオーディオソース")]
    [SerializeField]
    private AudioSource _recordAudioSource;
    [Header("曲データ")]
    [SerializeField]
    private MusicDatas _musicDatas;
    [Header("通常BGM")]
    [SerializeField]
    private AudioClip _mainAudioClip;

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

    protected override void Init()
    {
        base.Init();
        _dataLength = _musicDatas.GetCount();
        PlayMainBGM();
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventPlay();
        SetEventPlayTime();
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
            .Select(_ => _bgmAudioSource.time)
            .DistinctUntilChanged()
            .Subscribe(time =>
            {
                _playTime.Value = (int)time;
            });
    }

    private void SetEventState()
    {
        GameStateManager.MuseumStatus
            .TakeUntilDestroy(this)
            .Select(value => value == MuseumState.Music)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                if(value)
                {
                    //TargetAsync(_recordAudioSource, Ct).Forget();
                }
                else
                {
                    Pause();
                }
            });
    }

    /// <summary>
    /// 再生
    /// </summary>
    public void Play()
    {
        if (_bgmAudioSource.isPlaying)
            return;

        _bgmAudioSource.Play();
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
        _bgmAudioSource.clip = clip;
        _length.Value = (int)clip.length;
        _bgmAudioSource.Play();
    }

    /// <summary>
    /// メインのBGM再生
    /// </summary>
    public void PlayMainBGM()
    {
        Play(_mainAudioClip);
    }

    /// <summary>
    /// 次の曲を再生
    /// </summary>
    public void PlayNext()
    {
        Play(_index.Value + 1);
    }

    /// <summary>
    /// 前の曲を再生
    /// </summary>
    public void PlayBack()
    {
        Play(_index.Value - 1);
    }

    /// <summary>
    /// 停止
    /// </summary>
    public void Pause()
    {
        if (!_bgmAudioSource.isPlaying)
            return;

        _bgmAudioSource.Pause();
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
        if(_bgmAudioSource.mute == isMute)
            return;

        _bgmAudioSource.mute = isMute;
    }

    /// <summary>
    /// ループ設定
    /// </summary>
    /// <param name="isLoop"> ループするか </param>
    public void SetLoop(bool isLoop)
    {
        if (_bgmAudioSource.loop == isLoop)
            return;

        _bgmAudioSource.loop = isLoop;
    }

    /// <summary>
    /// 再生時間を設定
    /// </summary>
    /// <param name="time"></param>
    public void SetTime(float time)
    {
        _bgmAudioSource.time = time;
    }
}

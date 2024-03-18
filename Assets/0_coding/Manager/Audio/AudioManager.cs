using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// オーディオを管理
/// </summary>
public class AudioManager : DontDestroySingletonObject<AudioManager>
{
    private const int SOUND_INDEX = 4;
    private const string MASTER_VOLUME_NAME = "Master";
    private const string BGM_VOLUME_NAME = "BGM";
    private const string SE_VOLUME_NAME = "SE";

    private float[] _volumes = new float[SOUND_INDEX];
    [Header("オーディオミキサー")]
    [SerializeField]
    private AudioMixer _audioMixer;
    [Header("BGMのオーディオソース")]
    [SerializeField]
    private AudioSource _bgmAudioSource;
    [Header("SEのオーディオソース")]
    [SerializeField]
    private AudioSource _seAudioSource;
    [Header("通常BGM")]
    [SerializeField]
    private AudioClip _mainAudioClip;
    [Header("よく使うSE")]
    [SerializeField]
    private List<SE> _seList = new List<SE>();

    private bool _isMuteVideo = false;
    private bool _isMuteRecord = false;
    private AudioSource _videoAudioSource => GameVideoManager.Instance.VideoAudioSource;
    private AudioSource _recordAudioSource => PhonographMusicPlayerManager.Instance.RecordAudioSource;
    private List<AudioSource> _seAudioSourceList = new List<AudioSource>();
    private Dictionary<SEType, AudioClip> _seDictionary = new Dictionary<SEType, AudioClip>();

    protected override void Init()
    {
        base.Init();
        GetSEDictionary();
        SetInitVolume();
        PlayMainBGM();
    }

    protected override void SetEvent()
    {
        SetEventAudio();
    }

    /// <summary>
    /// SEの辞書を取得
    /// </summary>
    private void GetSEDictionary()
    {
        foreach (var se in _seList)
            _seDictionary.Add(se.SEType, se.Clip);
    }

    /// <summary>
    /// 音量の初期値設定
    /// </summary>
    private void SetInitVolume()
    {
        _volumes = SaveManager.GetSoundVolume();
        _audioMixer.SetFloat(MASTER_VOLUME_NAME, GetAudioMixerVolume(_volumes[(int)AudioType.Master]));
        _audioMixer.SetFloat(BGM_VOLUME_NAME, GetAudioMixerVolume(_volumes[(int)AudioType.BGM]));
        _audioMixer.SetFloat(SE_VOLUME_NAME, GetAudioMixerVolume(_volumes[(int)AudioType.SE]));
        _videoAudioSource.volume = GetAudioSourceVolume(_volumes[(int)AudioType.Video]);
    }

    /// <summary>
    /// オーディオミキサーに設定する音量
    /// </summary>
    /// <param name="volume"> 音量 </param>
    /// <returns></returns>
    private float GetAudioMixerVolume(float volume)
    {
        return -80 + volume * 10;
    }

    /// <summary>
    /// オーディオソースに設定する音量
    /// </summary>
    /// <param name="volume"> 音量 </param>
    /// <returns></returns>
    private float GetAudioSourceVolume(float volume)
    {
        return volume * _volumes[(int)AudioType.Master] / 100;
    }

    /// <summary>
    /// ステートごとの音量設定
    /// </summary>
    private void SetEventAudio()
    {
        GameStateManager.MuseumStatus
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                switch (value)
                {
                    case MuseumState.Play:
                        SetMute(false);
                        break;
                    case MuseumState.Pause:
                        SetMute(true, AudioType.Video);
                        SetMute(true, AudioType.Record);
                        break;
                    case MuseumState.Target:
                        SetMute(true, AudioType.Video);
                        SetMute(true, AudioType.Record);
                        break;  
                    case MuseumState.Video:
                        SetMute(true, AudioType.BGM);
                        SetMute(true, AudioType.Record);
                        break;
                    case MuseumState.Record:
                        SetMute(true, AudioType.BGM);
                        SetMute(true, AudioType.Video);
                        break;
                }
            });
    }

    /// <summary>
    /// メインBGM再生
    /// </summary>
    public void PlayMainBGM()
    {
        if (_bgmAudioSource.clip == null)
            _bgmAudioSource.clip = _mainAudioClip;
        if (_bgmAudioSource.isPlaying)
            return;

        _bgmAudioSource.Play();
    }

    /// <summary>
    /// メインBGM停止
    /// </summary>
    public void PauseMainBGM()
    {
        if (!_bgmAudioSource.isPlaying)
            return;

        _bgmAudioSource.Pause();
    }

    /// <summary>
    /// SEを鳴らす
    /// </summary>
    /// <param name="clip"> 鳴らすSE </param>
    public void PlayOneShotSE(AudioClip clip)
    {
        foreach (AudioSource se in _seAudioSourceList)
        {
            if(!se.isPlaying)
            {
                se.PlayOneShot(clip);
                return;
            }
        }

        CreateSEAudioSource();
        PlayOneShotSE(clip);
    }

    /// <summary>
    /// SEを鳴らす
    /// </summary>
    /// <param name="type"> Seの種類 </param>
    public void PlayOneShotSE(SEType type)
    {
        if(type == SEType.None)
            return;

        foreach (AudioSource se in _seAudioSourceList)
        {
            if (!se.isPlaying)
            {
                se.PlayOneShot(_seDictionary[type]);
                return;
            }
        }

        CreateSEAudioSource();
        PlayOneShotSE(type);
    }

    /// <summary>
    /// SEのオーディオソースを生成
    /// </summary>
    private void CreateSEAudioSource()
    {
        var seSource = Instantiate(_seAudioSource).GetComponent<AudioSource>();
        _seAudioSourceList.Add(seSource);
    }

    /// <summary>
    /// ミュート設定
    /// </summary>
    /// <param name="isMute"> ミュートにするか </param>
    public void SetMute(bool isMute)
    {
        _bgmAudioSource.mute = isMute;
        _seAudioSource.mute = isMute;
        if(!_isMuteVideo)
            _videoAudioSource.mute = isMute;
        if(!_isMuteRecord)
            _recordAudioSource.mute = isMute;
    }

    /// <summary>
    /// ミュート設定
    /// </summary>
    /// <param name="isMute"> ミュートにするか </param>
    public void SetMute(bool isMute, AudioType type)
    {
        switch (type)
        {
            case AudioType.Master:
                SetMute(isMute);
                break;
            case AudioType.BGM:
                _bgmAudioSource.mute = isMute;
                break;
            case AudioType.SE:
                _seAudioSource.mute = isMute;
                break;
            case AudioType.Video:
                if (GameStateManager.MuseumStatus.Value == MuseumState.Video)
                {
                    _isMuteVideo = isMute;
                    _videoAudioSource.mute = isMute;
                    break;
                }

                if(_isMuteVideo)
                {
                    break;
                }

                _videoAudioSource.mute = isMute;
                break;
            case AudioType.Record:
                _recordAudioSource.mute = isMute;
                if (GameStateManager.MuseumStatus.Value == MuseumState.Record)
                {
                    _isMuteRecord = isMute;
                    _recordAudioSource.mute = isMute;
                    break;
                }

                if(_isMuteRecord)
                {
                    break;
                }

                _recordAudioSource.mute = isMute;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 音量取得
    /// </summary>
    /// <returns></returns>
    public float[] GetSoundVolumes()
    {
        return _volumes;
    }

    /// <summary>
    /// 音量取得
    /// </summary>
    /// <returns></returns>
    public float GetSoundVolume(AudioType type)
    {
        return _volumes[(int)type];
    }

    /// <summary>
    /// 音量設定
    /// </summary>
    /// <param name="type"> オーディオの種類 </param>
    /// <param name="volume"> 音量 </param>
    public void SetVolume(AudioType type, float volume)
    {
        switch(type)
        {
            case AudioType.Master:
                _audioMixer.SetFloat(MASTER_VOLUME_NAME, GetAudioMixerVolume(volume));
                _volumes[(int)type] = volume;
                SetVolume(AudioType.Video, _volumes[(int)AudioType.Video]);
                return;
            case AudioType.BGM:
            case AudioType.Record:
                _audioMixer.SetFloat(BGM_VOLUME_NAME, GetAudioMixerVolume(volume));
                break;
            case AudioType.SE:
                _audioMixer.SetFloat(SE_VOLUME_NAME, GetAudioMixerVolume(volume));
                break;
            case AudioType.Video:
                _videoAudioSource.volume = GetAudioSourceVolume(volume);
                break;
            default:
                return;
        }

        _volumes[(int)type] = volume;
    }

    /// <summary>
    /// すべてのボリュームをセーブ
    /// </summary>
    public void SaveVolume()
    {
        SaveManager.SetSoundVolume(_volumes);
    }
}

[System.Serializable]
public class SE
{
    public SEType SEType;
    public AudioClip Clip;
}

/// <summary>
/// オーディオの種類
/// </summary>
public enum AudioType
{
    Master = 0,
    BGM = 1,
    SE = 2,
    Video = 3,
    Record = 4
}

/// <summary>
/// SEの種類
/// </summary>
public enum SEType
{
    None,
    Posi,
    Nega
}
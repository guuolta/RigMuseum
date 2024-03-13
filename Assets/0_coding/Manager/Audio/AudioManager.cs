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
    [Header("動画のオーディオソース")]
    [SerializeField]
    private AudioSource _movieAudioSource;
    private List<AudioSource> _seAudioSourceList = new List<AudioSource>();
    [Header("通常BGM")]
    [SerializeField]
    private AudioClip _mainAudioClip;
    [Header("よく使うSE")]
    [SerializeField]
    private List<SE> _seList = new List<SE>();
    private Dictionary<SEType, AudioClip> _seDictionary = new Dictionary<SEType, AudioClip>();

    protected override void Init()
    {
        base.Init();
        GetSEDictionary();
        SetInitVolume();
        PlayBGMAudioClip(_mainAudioClip);
    }

    protected override void SetEvent()
    {
        SetEventAudio();
    }

    /// <summary>
    /// BGM設定
    /// </summary>
    /// <param name="clip">曲</param>
    public void PlayBGMAudioClip(AudioClip clip)
    {
        _bgmAudioSource.clip = clip;
        _bgmAudioSource.Play();
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
    /// SEの辞書を取得
    /// </summary>
    private void GetSEDictionary()
    {
        foreach(var se in _seList)
            _seDictionary.Add(se.SEType, se.Clip);
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
        return volume * _volumes[(int)AudioType.Master] /100;
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
    /// 音量の初期値設定
    /// </summary>
    private void SetInitVolume()
    {
        _volumes = SaveManager.GetSoundVolume();
        _audioMixer.SetFloat(MASTER_VOLUME_NAME, GetAudioMixerVolume(_volumes[(int)AudioType.Master]));
        _audioMixer.SetFloat(BGM_VOLUME_NAME , GetAudioMixerVolume(_volumes[(int)AudioType.BGM]));
        _audioMixer.SetFloat(SE_VOLUME_NAME, GetAudioMixerVolume(_volumes[(int)AudioType.SE]));
        _movieAudioSource.volume = GetAudioSourceVolume(_volumes[(int)AudioType.Movie]);
    }

    /// <summary>
    /// マスターボリュームを設定
    /// </summary>
    /// <param name="volume">音量</param>
    public void SetMasterVolume(float volume)
    {
        _audioMixer.SetFloat(MASTER_VOLUME_NAME, GetAudioMixerVolume(volume));
        _volumes[(int)AudioType.Master] = volume;
        SetMovieVolume(_volumes[(int)AudioType.Movie]);
    }

    /// <summary>
    /// BGMボリュームを設定
    /// </summary>
    /// <param name="volume">音量</param>
    public void SetBGMVolume(float volume)
    {
        _audioMixer.SetFloat(BGM_VOLUME_NAME, GetAudioMixerVolume(volume));
        _volumes[(int)AudioType.BGM] = volume;
    }

    /// <summary>
    /// SEボリュームを設定
    /// </summary>
    /// <param name="volume">音量</param>
    public void SetSEVolume(float volume)
    {
        _audioMixer.SetFloat(SE_VOLUME_NAME, GetAudioMixerVolume(volume));
        _volumes[(int)AudioType.SE] = volume;
    }

    /// <summary>
    /// 動画ボリュームを設定
    /// </summary>
    /// <param name="volume">音量</param>
    public void SetMovieVolume(float volume)
    {
        _movieAudioSource.volume = GetAudioSourceVolume(volume);
        _volumes[(int)AudioType.Movie] = volume;
    }

    /// <summary>
    /// ミュート設定
    /// </summary>
    /// <param name="isMute"> ミュートにするか </param>
    public void SetMute(bool isMute)
    {
        _bgmAudioSource.mute = isMute;
        _seAudioSource.mute = isMute;
        _movieAudioSource.mute = isMute;
    }

    /// <summary>
    /// ミュート設定
    /// </summary>
    /// <param name="isMute"> ミュートにするか </param>
    public void SetMute(bool isMute, AudioType type)
    {
        switch(type)
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
            case AudioType.Movie:
                _movieAudioSource.mute = isMute;
                break;
            default:
                break;
        }
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
                    case MuseumState.Monitor:
                        _bgmAudioSource.mute = true;
                        break;
                }
            });
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
/// 音量の種類
/// </summary>
public enum AudioType
{
    Master = 0,
    BGM = 1,
    SE = 2,
    Movie = 3
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
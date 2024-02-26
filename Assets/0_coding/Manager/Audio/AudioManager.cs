using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// オーディオを管理
/// </summary>
public class AudioManager : SingletonObjectBase<AudioManager>
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

    /// <summary>
    /// BGM設定
    /// </summary>
    /// <param name="clip">曲</param>
    public void PlayBGMAudioClip(AudioClip clip)
    {
        _bgmAudioSource.clip = clip;
        _bgmAudioSource.Play();
    }

    public void PlayOneShotSE(AudioClip clip)
    {
        foreach(AudioSource se in _seAudioSourceList)
        {
            if(!se.isPlaying)
            {
                se.PlayOneShot(clip);
                return;
            }
        }

        var seSource = Instantiate(_seAudioSource).GetComponent<AudioSource>();
        _seAudioSourceList.Add(seSource);
    }

    public void PlayOneShotSE(SEType type)
    {
        foreach (AudioSource se in _seAudioSourceList)
        {
            if (!se.isPlaying)
            {
                se.PlayOneShot(_seDictionary[type]);
                return;
            }
        }

        var seSource = Instantiate(_seAudioSource).GetComponent<AudioSource>();
        _seAudioSourceList.Add(seSource);
    }

    private void GetSEDictionary()
    {
        foreach(var se in _seList)
        {
            _seDictionary.Add(se.SEType, se.Clip);
        }
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
        SetMovieVolume(_volumes[(int)AudioType.Master]);
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
    Posi,
    Nega
}
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// オーディオを管理
/// </summary>
public class AudioManager : SingletonObejectBase<AudioManager>
{
    private const string MASTER_VOLUME_NAME = "Master";
    private const string BGM_VOLUME_NAME = "BGM";
    private const string SE_VOLUME_NAME = "SE";

    private float[] _volumes = new float[3];
    [Header("オーディオミキサー")]
    [SerializeField]
    private AudioMixer _audioMixer;
    [Header("BGMのオーディオソース")]
    [SerializeField]
    private AudioSource _bgmAudioSource;
    [Header("通常BGM")]
    [SerializeField]
    private AudioClip _mainAudioClip;

    public override void Init()
    {
        base.Init();
        SetInitVolume();
    }

    public override void SetEvent()
    {
        SetBGMAudioClip(_mainAudioClip);
    }
    
    /// <summary>
    /// オーディオミキサーに設定する音量
    /// </summary>
    /// <param name="volume"> 音量 </param>
    /// <returns></returns>
    private float GetSoundVolume(float volume)
    {
        return -80 + volume * 10;
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

        _audioMixer.SetFloat(MASTER_VOLUME_NAME, GetSoundVolume(_volumes[(int)AudioType.Master]));
        _audioMixer.SetFloat(BGM_VOLUME_NAME , GetSoundVolume(_volumes[(int)AudioType.BGM]));
        _audioMixer.SetFloat(SE_VOLUME_NAME, GetSoundVolume(_volumes[(int)AudioType.SE]));
    }

    /// <summary>
    /// オーディオクリップ設定
    /// </summary>
    /// <param name="clip">曲</param>
    public void SetBGMAudioClip(AudioClip clip)
    {
        _bgmAudioSource.clip = clip;
    }

    /// <summary>
    /// マスターボリュームを設定
    /// </summary>
    /// <param name="volume">音量</param>
    public void SetMasterVolume(float volume)
    {
        _audioMixer.SetFloat(MASTER_VOLUME_NAME, GetSoundVolume(volume));
        _volumes[(int)AudioType.Master] = volume;
    }

    /// <summary>
    /// BGMボリュームを設定
    /// </summary>
    /// <param name="volume">音量</param>
    public void SetBGMVolume(float volume)
    {
        _audioMixer.SetFloat(BGM_VOLUME_NAME, GetSoundVolume(volume));
        _volumes[(int)AudioType.BGM] = volume;
    }

    /// <summary>
    /// SEボリュームを設定
    /// </summary>
    /// <param name="volume">音量</param>
    public void SetSEVolume(float volume)
    {
        _audioMixer.SetFloat(SE_VOLUME_NAME, GetSoundVolume(volume));
        _volumes[(int)AudioType.SE] = volume;
    }


    /// <summary>
    /// すべてのボリュームをセーブ
    /// </summary>
    public void SaveVolume()
    {
        SaveManager.SetSoundVolume(_volumes);
    }
}

/// <summary>
/// 音量の種類
/// </summary>
public enum AudioType
{
    Master = 0,
    BGM = 1,
    SE = 2
}
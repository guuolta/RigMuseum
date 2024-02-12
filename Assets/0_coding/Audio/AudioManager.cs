using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private const string MASTER_VOLUME_NAME = "Master";
    private const string BGM_VOLUME_NAME = "BGM";
    private const string SE_VOLUME_NAME = "SE";

    public static AudioManager Instance;

    [Header("オーディオミキサー")]
    [SerializeField]
    private AudioMixer _audioMixer;
    [Header("BGMのオーディオソース")]
    [SerializeField]
    private AudioSource _bgmAudioSource;
    [Header("通常BGM")]
    [SerializeField]
    private AudioClip _mainAudioClip;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        SetBGMAudioClip(_mainAudioClip);
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
        _audioMixer.SetFloat(MASTER_VOLUME_NAME, volume);
    }

    /// <summary>
    /// BGMボリュームを設定
    /// </summary>
    /// <param name="volume">音量</param>
    public void SetBGMVolume(float volume)
    {
        _audioMixer.SetFloat(BGM_VOLUME_NAME, volume);
    }


    /// <summary>
    /// SEボリュームを設定
    /// </summary>
    /// <param name="volume">音量</param>
    public void SetSEVolume(float volume)
    {
        _audioMixer.SetFloat(SE_VOLUME_NAME, volume);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SaveManager : MonoBehaviour
{
    private const string SENSITIVITY_KEY = "Sensitivity";
    private const string IS_VERTICAL_REVERSE_KEY = "IsVerRev";
    private const string IS_HORIZONTAL_REVERSE_KEY = "IsHorRev";
    private const string MASTER_VOLUME_KEY = "Master";
    private const string BGM_VOLUME_KEY = "BGM";
    private const string SE_VOLUME_KEY = "SE";

    public static SaveManager Instance;

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

    private void OnDestroy()
    {
        Save();
    }

    /// <summary>
    /// セーブデータからマウス感度取得
    /// </summary>
    /// <returns>感度</returns>
    public float GetSensitivity()
    {
        return PlayerPrefs.GetFloat(SENSITIVITY_KEY, 0f);
    }

    /// <summary>
    /// セーブデータからマウス操作が上下反転しているか取得
    /// </summary>
    /// <returns>反転しているか</returns>
    public bool GetIsVerticalReverse()
    {
        int value = PlayerPrefs.GetInt(IS_VERTICAL_REVERSE_KEY, 0);
        return value > 0;
    }

    /// <summary>
    /// セーブデータからマウス操作が左右反転しているか取得
    /// </summary>
    /// <returns>反転しているか</returns>
    public bool GetIsHorizontalReverse()
    {
        int value = PlayerPrefs.GetInt(IS_HORIZONTAL_REVERSE_KEY, 0);
        return value > 0;
    }

    /// <summary>
    /// セーブデータからマスター音量取得
    /// </summary>
    /// <returns>音量</returns>
    public float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 8f);
    }

    /// <summary>
    /// セーブデータからBGM音量取得
    /// </summary>
    /// <returns>音量</returns>
    public float GetBGMVolume()
    {
        return PlayerPrefs.GetFloat(BGM_VOLUME_KEY, 8f);
    }

    /// <summary>
    /// セーブデータからSE音量取得
    /// </summary>
    /// <returns>音量</returns>
    public float GetSEVolume()
    {
        return PlayerPrefs.GetFloat(SE_VOLUME_KEY, 8f);
    }

    /// <summary>
    /// セーブデータにマウス感度設定
    /// </summary>
    public void SetSaveSensitivity(float value)
    {
        PlayerPrefs.SetFloat(SENSITIVITY_KEY, value);
    }

    /// <summary>
    /// セーブデータにマウス操作が上下反転しているか設定
    /// </summary>
    public void SetSaveIsVerticalReverse(bool isReversed)
    {
        PlayerPrefs.SetInt(IS_VERTICAL_REVERSE_KEY, isReversed ? 1 : 0);
    }

    /// <summary>
    /// セーブデータにマウス操作が上下反転しているか設定
    /// </summary>
    public void SetSaveIsHorizontalReverse(bool isReversed)
    {
        PlayerPrefs.SetInt(IS_HORIZONTAL_REVERSE_KEY, isReversed ? 1 : 0);
    }

    /// <summary>
    /// セーブデータにマスター音量設定
    /// </summary>
    public void SetSaveMasterVolume(float volume)
    {
        PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
    }

    /// <summary>
    /// セーブデータにBGM音量設定
    /// </summary>
    public void SetSaveBGMVolume(float volume)
    {
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, volume);
    }

    /// <summary>
    /// セーブデータにSE音量設定
    /// </summary>
    public void SetSaveSEVolume(float volume)
    {
        PlayerPrefs.SetFloat(SE_VOLUME_KEY, volume);
    }

    /// <summary>
    /// セーブ
    /// </summary>
    public void Save()
    {
        PlayerPrefs.Save();
    }
}

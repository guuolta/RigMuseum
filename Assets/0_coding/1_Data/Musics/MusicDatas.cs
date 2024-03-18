using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicDatas", menuName = "ScriptableObjects/CreateMusicDatas")]
public class MusicDatas : ScriptableObjectBase<MusicData>
{
    
}

/// <summary>
/// 音楽のデータ
/// </summary>
[System.Serializable]
public class MusicData
{
    private int _id = -1;
    /// <summary>
    /// 音楽番号
    /// </summary>
    public int ID => _id;
    [Header("音楽名")]
    [SerializeField]
    private string _name;
    /// <summary>
    /// 音楽名
    /// </summary>
    public string Name => _name;
    [Header("音楽")]
    [SerializeField]
    private AudioClip _music;
    /// <summary>
    /// 音楽
    /// </summary>
    public AudioClip Music => _music;
    [Header("製作者")]
    [SerializeField]
    private string[] _members = new string[0];
    /// <summary>
    /// 製作者
    /// </summary>
    public string[] Members => _members;
    [Header("音楽の説明")]
    [TextArea(1, 10)]
    [SerializeField]
    private string _description;
    /// <summary>
    /// 音楽の説明
    /// </summary>
    public string Description => _description;

    public void SetID(int id)
    {
        _id = id;
    }
}
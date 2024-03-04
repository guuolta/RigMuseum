using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDatas", menuName = "ScriptableObjects/CreateGameDatas")]
public class GameDatas : ScriptableObjectBase<GameData>
{
    
}

[System.Serializable]
public class GameData
{
    [Header("ゲーム名")]
    [SerializeField]
    private string _title = "";
    /// <summary>
    /// ゲーム名
    /// </summary>
    public string Title => _title;
    [Header("紹介文")]
    [TextArea(1, 10)]
    [SerializeField]
    private string _description = "";
    /// <summary>
    /// ゲームの説明
    /// </summary>
    public string Description => _description;
    [Header("製作メンバー(Coading)")]
    [SerializeField]
    private string[] _coadingMenber = new string[0];
    /// <summary>
    /// 製作メンバー(Coading)
    /// </summary>
    public string[] CoadingMenber => _coadingMenber;
    [Header("製作メンバー(3D)")]
    [SerializeField]
    private string[] _modelMenber = new string[0];
    /// <summary>
    /// 製作メンバー(3D)
    /// </summary>
    public string[] ModelMenber => _modelMenber;
    [Header("製作メンバー(2D)")]
    [SerializeField]
    private string[] _illustrationMenber = new string[0];
    /// <summary>
    /// 製作メンバー(2D)
    /// </summary>
    public string[] IllustrationMenber => _illustrationMenber;
    [Header("製作メンバー(DTM)")]
    [SerializeField]
    private string[] _dtmMenber = new string[0];
    /// <summary>
    /// 製作メンバー(DTM)
    /// </summary>
    public string[] DTMMenber => _dtmMenber;
    [Header("ゲームのURｌ(UnityRoomなど)")]
    [SerializeField]
    private string _gameURL = "";
    /// <summary>
    /// ゲームのURｌ(UnityRoomなど)
    /// </summary>
    public string GameURL => _gameURL;
    [Header("紹介動画のYouTubeのURL(e.g. https://www.youtube.com/watch?v=VIDEO_ID)")]
    [SerializeField] 
    private string _youtubeURL = "";
    /// <summary>
    /// 紹介動画のYouTubeのURL(e.g. https://www.youtube.com/watch?v=VIDEO_ID)
    /// </summary>
    public string YoutubeURL => _youtubeURL;
}

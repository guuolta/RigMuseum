using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDatas", menuName = "ScriptableObjects/CreateGameDatas")]
public class GameDatas : ScriptableObject
{
    public List<GameData> GameDataList = new List<GameData>();
}

[System.Serializable]
public class GameData
{
    [Header("ゲーム名")]
    public string GameName;
    [Header("紹介文")]
    public string GameIntroduction;
    [Header("製作メンバー(Coading)")]
    public List<string> CoadingMenber = new List<string>();
    [Header("製作メンバー(3D)")]
    public List<string> ModelMenber = new List<string>();
    [Header("製作メンバー(2D)")]
    public List<string> IllustrationMenber = new List<string>();
    [Header("製作メンバー(DTM)")]
    public List<string> DTMMenber = new List<string>();
    [Header("ゲームのURｌ(UnityRoomなど)")]
    public string GameURL;
    [Header("紹介動画のYouTubeのURL(e.g. https://www.youtube.com/watch?v=VIDEO_ID)")]
    public string YoutubeURL;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDatas", menuName = "ScriptableObjects/CreateGameDatas")]
public class GameDatas : ScriptableObject
{
    [SerializeField]
    private List<GameData> _gameDataList = new List<GameData>();

    /// <summary>
    /// ゲームデータを取得
    /// </summary>
    /// <param name="index"> 要素番号 </param>
    /// <returns></returns>
    public GameData GetGameData(int index)
    {
        if (_gameDataList.Count == 0)
        {
            Debug.Log("スクリプタブルオブジェクトに値を設定してください");
            return null;
        }

        if (index > _gameDataList.Count)
        {
            Debug.Log("out of index");
            return _gameDataList[0];
        }

        return _gameDataList[index];
    }

    /// <summary>
    /// YouTubeURLを取得
    /// </summary>
    /// <param name="index"> 要素番号 </param>
    /// <returns></returns>
    public string GetGameYoutubeURL(int index)
    {
        var data = GetGameData(index);

        if(data == null)
        {
            return null;
        }

        return data.YoutubeURL;
    }
}

[System.Serializable]
public class GameData
{
    [Header("ゲーム名")]
    public string GameName;
    [Header("紹介文")]
    [TextArea(1, 10)]
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

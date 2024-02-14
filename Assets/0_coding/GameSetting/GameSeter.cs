using UnityEngine;

/// <summary>
/// ゲームの設定
/// </summary>
public class GameSeter : MonoBehaviour
{
    [Header("fpsの量")]
    [SerializeField]
    private int _fps = 60;

    private void Awake()
    {
        Application.targetFrameRate = _fps;
    }

    private void OnDestroy()
    {
        SaveManager.Save();
    }
}

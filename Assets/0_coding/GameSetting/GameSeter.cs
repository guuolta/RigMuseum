using UnityEngine;

/// <summary>
/// ゲームの設定
/// </summary>
public class GameSeter : ObjectBase
{
    [Header("fpsの量")]
    [SerializeField]
    private int _fps = 60;

    public override void Init()
    {
        Application.targetFrameRate = _fps;
        GameStateManager.SetMuseumState(MuseumState.Play);
    }

    public override void Destroy()
    {
        SaveManager.Save();
    }
}

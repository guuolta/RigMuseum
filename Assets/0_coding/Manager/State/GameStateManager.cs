using UniRx;

/// <summary>
/// ステートを管理
/// </summary>
public static class GameStateManager
{
    private static MuseumStateReactiveProperty _museumStatus = new MuseumStateReactiveProperty(MuseumState.Play);
    /// <summary>
    /// ステート
    /// </summary>
    public static IReadOnlyReactiveProperty<MuseumState> MuseumStatus { get { return _museumStatus; } }



    /// <summary>
    /// ポーズ状態を反転する
    /// </summary>
    public static void TogglePauseState()
    {
        _museumStatus.Value = _museumStatus.Value == MuseumState.Pause ? MuseumState.Play : MuseumState.Pause;
    }

    /// <summary>
    /// ステートを設定
    /// </summary>
    /// <param name="state"> ステート </param>
    public static void SetMuseumState(MuseumState state)
    {
        _museumStatus.Value = state;
    }
}


/// <summary>
/// ステート一覧
/// </summary>
public enum MuseumState
{
    None,
    Play,
    Pause,
    Caption,
    Music,
    Monitor
}

[System.Serializable]
public class MuseumStateReactiveProperty : ReactiveProperty<MuseumState>
{
    public MuseumStateReactiveProperty() { }
    public MuseumStateReactiveProperty(MuseumState initialValue) : base(initialValue) { }
}
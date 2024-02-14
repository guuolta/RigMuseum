using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// ステートを管理
/// </summary>
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    private MuseumStateReactiveProperty _museumStatus = new MuseumStateReactiveProperty(MuseumState.Play);
    /// <summary>
    /// ステート
    /// </summary>
    public IReadOnlyReactiveProperty<MuseumState> MuseumStatus { get { return _museumStatus; } }

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
        SetMuseumState(MuseumState.Play);
    }

    /// <summary>
    /// ポーズ状態を反転する
    /// </summary>
    public void TogglePauseState()
    {
        _museumStatus.Value = _museumStatus.Value == MuseumState.Pause ? MuseumState.Play : MuseumState.Pause;
    }

    /// <summary>
    /// ステートを設定
    /// </summary>
    /// <param name="state"> ステート </param>
    public void SetMuseumState(MuseumState state)
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
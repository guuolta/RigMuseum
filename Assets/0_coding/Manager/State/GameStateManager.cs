using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    private MuseumStateReactiveProperty _museumStatus = new MuseumStateReactiveProperty(MuseumState.Play);
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

    public void SetMuseumState(MuseumState state)
    {
        _museumStatus.Value = state;
    }
}

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
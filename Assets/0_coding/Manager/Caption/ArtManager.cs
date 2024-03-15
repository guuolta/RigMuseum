public class ArtManager : ProductionManagerBase<ArtManager>
{
    public void SetState()
    {
        GameStateManager.SetMuseumState(MuseumState.Target);
    }
}
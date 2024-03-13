public class CaptionManager : ProductionManagerBase<CaptionManager>
{
    public void SetState()
    {
        GameStateManager.SetMuseumState(MuseumState.Target);
    }
}
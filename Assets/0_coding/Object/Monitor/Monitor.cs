public class Monitor : TouchObjectBase
{
    public override void StartTouchEvent()
    {
        base.StartTouchEvent();
        GameStateManager.SetMuseumState(MuseumState.Monitor);
    }
}

public class SingletonObejectBase<T> : ObjectBase
    where T : ObjectBase, new()
{
    public static T Instance;

    public override void Init()
    {
        if (Instance == null)
        {
            Instance = new T();
        }
        else
        {
            Destroy(this);
        }
    }
}

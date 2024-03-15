using UnityEngine;
using UnityEngine.EventSystems;
using UniRx;
using Cysharp.Threading.Tasks;

public abstract class TouchObjectBase : GameObjectBase
{
    [Header("名前UIのオブジェクト名(_nameUIが設定されている場合は不要)")]
    [SerializeField]
    private string NameUIName;
    [Header("方向")]
    [SerializeField]
    private TargetDirection _direction = TargetDirection.None;
    public TargetDirection Direction => _direction;
    [Header("SE")]
    [SerializeField]
    private SEType _seType= SEType.Posi;

    [Header("名前UI(名前UIのオブジェクト名が設定されている場合は不要)")]
    [SerializeField]
    private ObjectNamePanelPresenter _nameUI;
    /// <summary>
    /// 名前UI
    /// </summary>
    protected ObjectNamePanelPresenter NameUI
    {
        get
        {
            if (_nameUI == null)
            {
                var obj = GameObject.Find(NameUIName);
                if(obj != null)
                {
                    _nameUI = obj.GetComponent<ObjectNamePanelPresenter>();
                }
            }

            return _nameUI;
        }
    }

    private bool _isTouch;

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventTouch();
    }

    /// <summary>
    /// オブジェクトをクリックした時のイベント
    /// </summary>
    public virtual async void StartTouchEvent()
    {
        if (_isTouch)
            return;

        _isTouch = true;
        AudioManager.Instance.PlayOneShotSE(_seType);
        await NameUI.HideAsync(Ct);
    }

    protected virtual async void OnMouseEnter()
    {
        if(_isTouch)
        {
            return;
        }

        if(NameUI == null)
        {
            Debug.Log("名前UIがありません");
            return;
        }

        if(EventSystem.current.IsPointerOverGameObject())
        {
            await NameUI.HideAsync(Ct);
        }

        await NameUI.ShowAsync(Ct);
    }

    protected virtual async void OnMouseExit()
    {
        if (_isTouch)
        {
            return;
        }

        if (NameUI == null)
        {
            Debug.Log("名前UIがありません");
            return;
        }

        await NameUI.HideAsync(Ct);
    }

    /// <summary>
    /// 名前UIを設定
    /// </summary>
    /// <param name="nameUI"></param>
    public void SetNameUI(string name)
    {
        NameUIName = name;
    }

    /// <summary>
    /// ターゲット状態解除
    /// </summary>
    public void ClearTouch()
    {
        _isTouch = false;
    }

    /// <summary>
    /// タッチ状態のイベントを設定
    /// </summary>
    protected virtual void SetEventTouch()
    {
        GameStateManager.MuseumStatus
            .TakeUntilDestroy(this)
            .Where(value => value != MuseumState.Target)
            .Select(value => value != MuseumState.Play)
            .Subscribe((value =>
            {
                _isTouch = value;

                if(_isTouch)
                {
                    NameUI.HideAsync(Ct).Forget();
                }
            }));
    }
}

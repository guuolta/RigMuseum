using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class ToggleBase : UIPartBase
{
    private BoolReactiveProperty _isToggle = new BoolReactiveProperty(false);
    /// <summary>
    /// トグルのOn、Off
    /// </summary>
    public BoolReactiveProperty IsToggle => _isToggle;
    private Image _toggle;
    /// <summary>
    /// トグルの画像
    /// </summary>
    public Image Toggle
    {
        get
        {
            if( _toggle == null )
            {
                _toggle = GetComponent<Image>();
            }

            return _toggle;
        }
    }
    [Header("トグルのマスク画像")]
    [SerializeField]
    private Image _mask;

    public override void SetEvent()
    {
        base.SetEvent();
        SetEventToggle();
        SetEventPointer(Toggle);
    }

    public override void SetEventOnClick(Image image)
    {
        image.OnPointerClickAsObservable()
            .Subscribe(_ =>
            {
                _isToggle.Value = !_isToggle.Value;
            }).AddTo(this);
    }

    public void SetEventToggle()
    {
        IsToggle
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                if(value)
                {
                    _mask.gameObject.SetActive(false);
                }
                else
                {
                    _mask.gameObject.SetActive(true);
                }
            }).AddTo(this);
    }

    public void SetValue(bool value)
    {
        _isToggle.Value = value;
    }
}

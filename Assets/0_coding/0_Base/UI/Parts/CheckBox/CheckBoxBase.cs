using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheckBoxBase : UIAnimationPartBase
{
    private BoolReactiveProperty _isCheck = new BoolReactiveProperty(false);
    /// <summary>
    /// チェックボックスのOn、Off
    /// </summary>
    public BoolReactiveProperty IsCheck => _isCheck;
    private Image _checkBoxBackImage;
    /// <summary>
    /// チェックボックスの背景画像
    /// </summary>
    public Image CheckBoxBackImage
    {
        get
        {
            if( _checkBoxBackImage == null )
            {
                _checkBoxBackImage = GetComponent<Image>();
            }

            return _checkBoxBackImage;
        }
    }
    [Header("チェックボックスのマスク画像")]
    [SerializeField]
    private Image _mask;

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventCheckBox();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        _isCheck.Value = !_isCheck.Value;
        AudioManager.Instance.PlayOneShotSE(_isCheck.Value ? SEType.Posi : SEType.Nega);
    }

    public void SetEventCheckBox()
    {
        IsCheck
            .TakeUntilDestroy(this)
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
            });
    }

    public void SetValue(bool value)
    {
        _isCheck.Value = value;
    }
}

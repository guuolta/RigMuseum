using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpeedPanelCell : UIPartBase
{
    private BoolReactiveProperty _isCheck = new BoolReactiveProperty(false);
    /// <summary>
    /// チェックされているか
    /// </summary>
    public BoolReactiveProperty IsCheck => _isCheck;
    private Image _cell;
    protected Image cell
    {
        get
        {
            if(_cell == null)
            {
                _cell = GetComponent<Image>();
            }

            return _cell;
        }
    }
    [Header("チェックマーク")]
    [SerializeField]
    private Image _check = null;

    protected override void Init()
    {
        Hide(cell);
        if(_check != null)
        {
            Hide(_check);
        }
    }

    protected override void SetEvent()
    {
        SetEventCell();
    }

    public void ShowCell(Image image)
    {
        Color newColor = image.color;
        newColor.a = 1;
        image.color = newColor;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (IsCheck.Value)
        {
            return;
        }

        ShowCell(cell);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if(IsCheck.Value)
        {
            return;
        }

        Hide(cell);
    }

    public void DisCheck()
    {
        if(!_isCheck.Value)
        {
            return;
        }

        _isCheck.Value = false;
    }

    private void SetEventCell()
    {
        if(_check == null)
        {
            return;
        }

        OnClickCallback += () =>
        {
            if(IsCheck.Value)
            {
                return;
            }

            _isCheck.Value = true;
        };

        IsCheck
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                if(value)
                {
                    ShowCell(cell);
                    Show(_check);
                }
                else
                {
                    Hide(cell);
                    Hide(_check);
                }
            });
    }
}

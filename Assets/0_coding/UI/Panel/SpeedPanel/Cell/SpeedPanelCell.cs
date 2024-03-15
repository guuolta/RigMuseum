using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpeedPanelCell : UIBase
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

    /// <summary>
    /// チェックを設定
    /// </summary>
    /// <param name="check"> チェックをつけるか </param>
    public void SetCheck(bool check)
    {
        _isCheck.Value = check;
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

using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MediaButton : ButtonBase
{
    [Header("上から順番に表示される")]
    [SerializeField]
    private List<MediaButtonData> _dataList = new List<MediaButtonData>();

    private int _activeIndex = 0;
    private int _listCount;
    private MediaButtonData _targetData;
    /// <summary>
    /// 対象のデータ
    /// </summary>
    protected MediaButtonData TargetData => _targetData;

    protected override void Init()
    {
        base.Init();

        _listCount = _dataList.Count;
        if (_listCount == 0)
        {
            Debug.LogError("There aren't data");
            return;
        }

        SetInitButtonImage();
        SetNextData();
    }

    protected override void SetEvent()
    {
        base.SetEvent();
        SetEventClick(Ct);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
    }

    public override async void OnPointerEnter(PointerEventData eventData)
    {
        await _targetData.ExplainText.ShowAsync(Ct);
    }

    public override async void OnPointerExit(PointerEventData eventData)
    {
        await _targetData.ExplainText.HideAsync(Ct);
    }

    /// <summary>
    /// アクティブな要素番号を取得
    /// </summary>
    /// <returns></returns>
    public int GetIndex()
    {
        return _activeIndex;
    }

    /// <summary>
    /// 次のデータを設定する
    /// </summary>
    protected void SetNextData()
    {
        _targetData = _dataList[_activeIndex];
        _activeIndex = (_activeIndex + 1) % _listCount;
    }

    /// <summary>
    /// ボタンの画像の初期設定
    /// </summary>
    private void SetInitButtonImage()
    {
        if (_dataList[0].ButtonImage == null)
            return;

        Show(_dataList[0].ButtonImage);
        for(int i = 1; i < _listCount; i++)
        {
            Hide(_dataList[i].ButtonImage);
        }
    }

    protected virtual void SetEventClick(CancellationToken ct)
    {
        if(_listCount <=1)
            return;

        OnClickCallback += async () =>
        {
            HideAsync(TargetData.ButtonImage, ct).Forget();
            TargetData.ExplainText.HideAsync(ct).Forget();
            SetNextData();
            await ShowAsync(TargetData.ButtonImage, ct);
        };
    }
}

/// <summary>
/// メディアボタンのデータ
/// </summary>
[System.Serializable]
public class MediaButtonData
{
    [Header("ボタンの画像")]
    [SerializeField]
    private Image _buttonImage;
    public Image ButtonImage => _buttonImage;
    [Header("ボタンの説明テキスト")]
    [SerializeField]
    private ExplainText _explainText;
    public ExplainText ExplainText => _explainText;
}
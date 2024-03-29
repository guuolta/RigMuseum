using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class SlidePanelManager : ObjectBase
{
    [Header("スライド一覧")]
    [SerializeField]
    private List<SliderPanelPresenter> _sliderPanelList = new List<SliderPanelPresenter>();
    [Header("右矢印ボタン")]
    [SerializeField]
    private ButtonBase _rightButton;
    [Header("左矢印ボタン")]
    [SerializeField]
    private ButtonBase _leftButton;

    private ReactiveProperty<int> _selectedIndex = new ReactiveProperty<int>(0);

    protected override void SetEvent()
    {
        SetEventButton();
    }

    /// <summary>
    /// ボタンのイベント設定
    /// </summary>
    private void SetEventButton()
    {
        _rightButton.OnClickCallback += async () =>
        {
            _sliderPanelList[_selectedIndex.Value].HideAsync(Ct).Forget();
            _selectedIndex.Value++;
            await _sliderPanelList[_selectedIndex.Value].ShowAsync(Ct);
        };

        _leftButton.OnClickCallback += async () =>
        {
            _sliderPanelList[_selectedIndex.Value].HideAsync(Ct).Forget();
            _selectedIndex.Value--;
            await _sliderPanelList[_selectedIndex.Value].ShowAsync(Ct);
        };

        _selectedIndex
            .Select(value => Mathf.Clamp(value, 0, _sliderPanelList.Count))
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                _leftButton.ChangeInteractive(_selectedIndex.Value != 0);
                _rightButton.ChangeInteractive(_selectedIndex.Value != _sliderPanelList.Count);
            }).AddTo(this);
    }
}

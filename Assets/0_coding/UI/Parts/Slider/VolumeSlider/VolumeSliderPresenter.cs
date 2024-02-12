using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class VolumeSliderPresenter : MonoBehaviour
{
    [SerializeField]
    private SliderBase _view;
    private void Awake()
    {
        if(_view == null)
        {
            Debug.Log("SliderViewBaseを設定してください");
        }
    }

    private void Start()
    {
        SetEventSlider();
        //_view.SetSliderValue(GetVolume());
    }

    /*スライダーの値が変わった時のイベント発行*/
    private void SetEventSlider()
    {
        _view.SliderValueAsObservable.Subscribe(value =>
        {
            SetVolume(value * 10 - 80);
            
        }).AddTo(this);
    }

    /// <summary>
    /// 初期値取得
    /// </summary>
    /// <returns>音量</returns>
    public virtual float GetVolume() { return 8f; }

    /// <summary>
    /// 音量設定
    /// </summary>
    /// <param name="volume">音量</param>
    public virtual void SetVolume(float volume) { }
}

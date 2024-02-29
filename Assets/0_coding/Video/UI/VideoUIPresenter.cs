using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Video;

public class VideoUIPresenter : PresenterBase<VideoUIView>
{
    protected override void Init()
    {
        View.ChangeInteractive(false);
    }

    protected override void SetEvent()
    {
        SetEventSeekBar();
        SetEventPlayButton(Ct);
        SetEventSpeedButton(Ct);
        SetEventMuteButton();
        SetEventVolumeSlider();
        SetEventVideoTime();
    }

    public override async UniTask ShowAsync(CancellationToken ct)
    {
        SetInitVolumeSliderValue();
        await base.ShowAsync(ct);
    }

    private void SetEventSeekBar()
    {
        View.SeekBar.SliderValueAsObservable
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {

            });

        GameVideoManager.Instance.VideoTime
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                View.SeekBar.SetSlider(0, value);
            });

        GameVideoManager.Instance.VideoPlayTime
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                View.SeekBar.SetValue(value);
            });
    }

    private void SetEventPlayButton(CancellationToken ct)
    {
        View.PlayButton.IsOn
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                if(value)
                {
                    GameVideoManager.Instance.Pause(ct).Forget();
                }
                else
                {
                    GameVideoManager.Instance.Play(ct).Forget();
                }
            });
    }

    private void SetEventSpeedButton(CancellationToken ct)
    {
        View.SpeedButton.OnClickCallback += async () =>
        {
            if(View.SpeedPanel.IsOpen)
            {
                await View.SpeedPanel.HideAsync(ct);
            }
            else
            {
                await View.SpeedPanel.ShowAsync(ct);
            }
        };

        View.SpeedPanel.OnIndex
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {

            });
    }

    private void SetEventMuteButton()
    {
        View.MuteButton.IsOn
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                GameVideoManager.Instance.Mute(value);    
            });
    }

    private void SetInitVolumeSliderValue()
    {
        View.VolumeSlider.SetValue(AudioManager.Instance.GetSoundVolume(AudioType.Movie));
    }

    private void SetEventVolumeSlider()
    {
        View.VolumeSlider.SliderValueAsObservable
            .Skip(1)
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                AudioManager.Instance.SetMovieVolume(value);
            });
            
    }

    public void SetEventVideoTime()
    {
        GameVideoManager.Instance.VideoTime
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                View.VideoTimeText.text = GetVideoTime(value);
            });

        GameVideoManager.Instance.VideoPlayTime
            .TakeUntilDestroy(this)
            .DistinctUntilChanged()
            .Subscribe(value =>
            {
                View.PlayTimeText.text = GetVideoTime(value);
            });
    }

    private string GetVideoTime(int time)
    {
        if(time < 0)
        {
            time = 0;
        }

        int minutes = time / 60;
        int seconds = time % 60;

        return $"{minutes:00}:{seconds:00}";
    }
}

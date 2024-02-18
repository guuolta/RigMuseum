using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using YoutubePlayer;
using System.Threading.Tasks;
using UniRx;
using Unity.VisualScripting;
using Cysharp.Threading.Tasks;

public class YoutubeVideoPlayer : ObjectBase
{
    private bool _isPrepareVideo = true;
    private VideoPlayer _videoPlayer;
    public VideoPlayer VideoPlayer
    {
        get
        {
            if(_videoPlayer == null)
            {
                _videoPlayer = GetComponent<VideoPlayer>();
                _videoPlayer.playOnAwake = false;
            }

            return _videoPlayer;
        }
    }
    private YoutubePlayer.YoutubePlayer _youtubePlayer;
    public YoutubePlayer.YoutubePlayer YoutubePlayer
    {
        get
        {
            if(_youtubePlayer == null)
            {
                _youtubePlayer = GetComponent<YoutubePlayer.YoutubePlayer>();
            }

            return _youtubePlayer;
        }
    }
    private AudioSource _audioSource;
    public AudioSource AudioSource
    {
        get
        {
            if( _audioSource == null)
            {
                _audioSource = VideoPlayer.GetTargetAudioSource(0);
            }

            return _audioSource;
        }
    }

    public override void SetEvent()
    {
        SetEventPlayVideo();
    }

    /// <summary>
    /// ビデオの再生設定
    /// </summary>
    private void SetEventPlayVideo()
    {
        GameStateManager.MuseumStatus
            .Subscribe(async value =>
            {
                await UniTask.WaitUntil(() => !_isPrepareVideo);

                switch (value)
                {
                    case MuseumState.Play:
                        AudioSource.mute = true;
                        VideoPlayer.Play();
                        break;
                    case MuseumState.Monitor:
                        AudioSource.mute = false;
                        VideoPlayer.Play();
                        break;
                    default: 
                        VideoPlayer.Pause();
                        break;
                }
            }).AddTo(this);
    }

    public async UniTask SetInitVideo(string youtubeURL)
    {
        _isPrepareVideo = true;
        await YoutubePlayer.PrepareVideoAsync(youtubeURL);
        _isPrepareVideo = false;
    }

    public async UniTask PlayVideo(string youtubeURL)
    {
        _isPrepareVideo = true;
        await YoutubePlayer.PlayVideoAsync(youtubeURL);
        _isPrepareVideo = false;
    }
}

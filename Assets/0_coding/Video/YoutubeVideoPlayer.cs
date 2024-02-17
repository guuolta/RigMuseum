using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using YoutubePlayer;
using System.Threading.Tasks;
using UniRx;
using Unity.VisualScripting;

public class YoutubeVideoPlayer : ObjectBase
{
    [Header("YoutubeのURL(e.g. https://www.youtube.com/watch?v=VIDEO_ID)")]
    [SerializeField]
    private List<string> _youtubeURLs = new List<string>();
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

    public override async void SetEvent()
    {
        await YoutubePlayer.PrepareVideoAsync(_youtubeURLs[0]);
        SetEventPlayVideo();
    }

    private void SetEventPlayVideo()
    {
        GameStateManager.MuseumStatus
            .Subscribe(value =>
            {
                switch(value)
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
}

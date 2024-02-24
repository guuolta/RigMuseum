using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

public class YoutubeVideoPlayer : ObjectBase
{
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

    /// <samarry>
    /// 設定されている動画再生
    /// </samarry>
    public void Play()
    {
        if(AudioSource.mute)
        {
            AudioSource.mute = false;
        }

        if(!VideoPlayer.isPlaying)
        {
            VideoPlayer.Play();
        }
    }

    /// <summary>
    /// 動画再生
    /// </summary>
    /// <param name="youtubeURL"> 再生する動画のURL </param>
    /// <returns></returns>
    public async UniTask Play(string youtubeURL)
    {
        await YoutubePlayer.PlayVideoAsync(youtubeURL);
    }

    /// <summary>
    /// 動画を止める
    /// </summary>
    public void Pause()
    {
        if (VideoPlayer.isPlaying)
        {
            VideoPlayer.Pause();
        }
    }
}

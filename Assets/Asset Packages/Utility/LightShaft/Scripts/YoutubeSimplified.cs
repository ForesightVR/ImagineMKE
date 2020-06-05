using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Foresight;

public class YoutubeSimplified : MonoBehaviour
{
    private YoutubePlayer player;

    public string url;
    public bool autoPlay = true;
    public bool fullscreen = true;
    private VideoPlayer videoPlayer;

    VideoRenderMode mode;

    private void Awake()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
        player = GetComponentInChildren<YoutubePlayer>();

        player.videoPlayer = videoPlayer;
        mode = videoPlayer.renderMode;
    }

    private void Start()
    {
        Play();
    }

    public void Play()
    {
        if (fullscreen)
        {
            videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        }
        player.autoPlayOnStart = autoPlay;
        player.videoQuality = YoutubePlayer.YoutubeVideoQuality.STANDARD;

        if(autoPlay)
            player.Play(url);
    }

    public void SetFullScreen()
    {
        if(player.mainCamera == null)
        {
            Debug.Log("Set Up Camera");
            videoPlayer.targetCamera = Player.LocalPlayerInstance.cam;
            player.mainCamera = Player.LocalPlayerInstance.cam;
        }

        if (videoPlayer.renderMode == mode)
            videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        else
            videoPlayer.renderMode = mode;
    }
}

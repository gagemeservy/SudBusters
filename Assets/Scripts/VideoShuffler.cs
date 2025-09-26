using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoShuffler : MonoBehaviour
{

    public VideoPlayer player;
    public RawImage rawImage;      // assign in inspector
    private string[] videoFilePaths;

    void Start()
    {
        if (player == null) player = GetComponent<VideoPlayer>();
        if (rawImage == null) Debug.LogWarning("RawImage not assigned!");

        // Load all video files from StreamingAssets
        videoFilePaths = Directory.GetFiles(Application.streamingAssetsPath, "*.*", SearchOption.TopDirectoryOnly);
        videoFilePaths = System.Array.FindAll(videoFilePaths, s =>
            s.EndsWith(".mp4") || s.EndsWith(".mov") || s.EndsWith(".webm") || s.EndsWith(".ogg")
        );

        if (videoFilePaths.Length == 0)
        {
            Debug.LogWarning("No video files found in StreamingAssets folder.");
            return;
        }

        player.prepareCompleted += OnVideoPrepared;
        player.loopPointReached += OnVideoEnd;

        PlayRandomClip();
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        PlayRandomClip();
    }

    void PlayRandomClip()
    {
        if (videoFilePaths.Length == 0) {
            Debug.Log("videoFilePaths.length is 0");
            return;
                }

        int index = Random.Range(0, videoFilePaths.Length);
        player.url = videoFilePaths[index];
        player.Prepare();  // Prepare triggers prepareCompleted
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        rawImage.texture = player.texture;

        player.isLooping = true;
        player.waitForFirstFrame = false;
        player.Play();
    }

}

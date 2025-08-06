using UnityEngine;
using UnityEngine.Video; // Required for VideoPlayer
using UnityEngine.UI; // Required if playing on a RawImage UI element

public class PlayVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Assign this in the Inspector
    public VideoClip videoClip;     // Assign your video file here
    public RawImage displayImage;   // Optional: Assign a UI RawImage to display the video

    void Start()
    {
        // If videoPlayer is not assigned, try to get it from the current GameObject
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // If a videoClip is assigned, set it as the source
        if (videoClip != null)
        {
            videoPlayer.clip = videoClip;
        }

        // Optional: If displaying on a RawImage, set its texture to the VideoPlayer's target texture
        if (displayImage != null && videoPlayer.targetTexture != null)
        {
            displayImage.texture = videoPlayer.targetTexture;
        }

        // Start playing the video
        videoPlayer.Play();
    }

    // Example of pausing the video
    public void PauseVideo()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
    }

    // Example of resuming the video
    public void ResumeVideo()
    {
        if (videoPlayer != null && videoPlayer.isPaused)
        {
            videoPlayer.Play();
        }
    }

    // Example of stopping the video
    public void StopVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }
    }
}
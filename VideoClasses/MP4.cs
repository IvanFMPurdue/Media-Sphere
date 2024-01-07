using System;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;


namespace Media_Sphere.VideoClasses
{
    public class MP4Player
    {
        private MediaElement mediaElement;
        private bool isPlaying;


        public MP4Player()
        {
            mediaElement = new MediaElement();
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.UnloadedBehavior = MediaState.Manual;
            mediaElement.MediaEnded += (sender, e) => isPlaying = false;
        }


        public void PlayMP4(string filePath)
        {
            mediaElement.Source = new Uri(filePath);
            mediaElement.Play();
            isPlaying = true;
        }


        public void PauseOrResume()
        {
            if (isPlaying)
            {
                mediaElement.Pause();
                isPlaying = false;
            }
            else
            {
                mediaElement.Play();
                isPlaying = true;
            }
        }


        public void Replay()
        {
            mediaElement.Stop();
            mediaElement.Play();
            isPlaying = true;
        }


        public MediaElement GetMediaElement()
        {
            return mediaElement;
        }
    }
}
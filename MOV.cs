using System;
using System.Windows.Controls;

namespace Media_Sphere
{
    public class MOVPlayer
    {
        private MediaElement mediaElement;
        private bool isPlaying;

        public MOVPlayer()
        {
            mediaElement = new MediaElement();
            mediaElement.LoadedBehavior = MediaState.Manual;
            mediaElement.UnloadedBehavior = MediaState.Manual;
            mediaElement.MediaEnded += (sender, e) => isPlaying = false;
        }

        public void PlayMOV(string filePath)
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

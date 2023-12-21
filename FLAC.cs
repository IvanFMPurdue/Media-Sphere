﻿using NAudio.Wave;
using System;

public class FLACPlayer
{
    private WaveOutEvent waveOut;
    private AudioFileReader audioFile;

    public void PlayFLAC(string filePath)
    {
        try
        {
            waveOut = new WaveOutEvent();
            audioFile = new AudioFileReader(filePath);

            waveOut.Init(audioFile);
            waveOut.Play();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error playing FLAC audio: {ex.Message}");
        }
    }

    public void Pause()
    {
        if (waveOut != null && waveOut.PlaybackState == PlaybackState.Playing)
        {
            waveOut.Pause();
        }
    }

    public void Resume()
    {
        if (waveOut != null && waveOut.PlaybackState == PlaybackState.Paused)
        {
            waveOut.Play();
        }
    }

    public void Stop()
    {
        if (waveOut != null)
        {
            waveOut.Stop();
            waveOut.Dispose();
        }

        if (audioFile != null)
        {
            audioFile.Dispose();
        }
    }

    public void PauseOrResume()
    {
        if (waveOut != null)
        {
            if (waveOut.PlaybackState == PlaybackState.Playing)
            {
                waveOut.Pause();
            }
            else if (waveOut.PlaybackState == PlaybackState.Paused)
            {
                waveOut.Play();
            }
        }
    }

    public void Replay()
    {
        if (waveOut != null)
        {
            waveOut.Stop();
            waveOut.Play();
        }
    }
}

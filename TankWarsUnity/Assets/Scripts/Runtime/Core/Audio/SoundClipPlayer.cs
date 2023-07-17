namespace TankWars.Runtime.Core.Audio
{
    using System;
    using TankWars.Runtime.Core.Tools.Time; 
    using TankWars.Runtime.Gameplay.ObjectManagement;
    using UnityEngine;

    [RequireComponent(typeof(AudioSource))]
    public class SoundClipPlayer : PoolObject
    {
        public event Action onClipFinishedPlaying; 

        private Timer audioClipTimer = null; 
        private AudioSource audioSource = null;

        public bool IsPlaying { get; private set; } = false; 

        #region Unity Methods

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        #endregion

        public void PlayNewClip(AudioClip audioClip, bool isSpatialSound, float volume = 1, bool playOnce = true)
        {
            audioSource.volume = volume;
            audioSource.spatialBlend = isSpatialSound ? 1 : 0;
            audioSource.clip = audioClip;
            audioSource.loop = !playOnce; 
            audioSource.Play();
            IsPlaying = true; 

            if(playOnce)
            {
                audioClipTimer = new Timer(audioClip.length, false);
                
                audioClipTimer.onTimerCompleted += () =>
                {
                    onClipFinishedPlaying?.Invoke();
                    audioClipTimer = null; 
                };

                audioClipTimer.Start(); 
            }
        }

        public void PlayCurrentClip()
        {
            audioSource.Play();
            IsPlaying = true; 
            audioClipTimer?.Start(); 
        }

        public void PauseCurrentClip()
        {
            audioSource.Pause();
            IsPlaying = false; 
            audioClipTimer?.Stop(); 
        }

        public void StopCurrentClip()
        {
            audioSource.Stop();
            onClipFinishedPlaying?.Invoke(); 
        }

        public void ChangeVolume(float volume)
        {
            audioSource.volume = volume; 
        }
    }
}

namespace TankWars.Runtime.Core.Audio
{
    using UnityEngine;

    public class NullAudioService : IAudioService
    {
        private readonly string missingAudioServiceMessage = $"The audio service has not been set, please make sure to call the {nameof(AudioManager.SetAudioServiceType)} method on the {typeof(AudioManager).Name} instance you have in the game.";

        public void PlaySound(AudioRequest audioRequest, float volume)
        {
            Debug.LogError(missingAudioServiceMessage);
        }

        public void PauseAllSounds()
        {
            Debug.LogError(missingAudioServiceMessage);
        }

        public void StopAllSounds()
        {
            Debug.LogError(missingAudioServiceMessage);
        }

        public void PlaySoundWithId(AudioRequest audioRequest, float volume)
        {
            Debug.LogError(missingAudioServiceMessage); 
        }

        public void PauseSoundWithId(AudioRequest audioRequest)
        {
            Debug.LogError(missingAudioServiceMessage); 
        }

        public void StopSoundWithId(AudioRequest audioRequest)
        {
            Debug.LogError(missingAudioServiceMessage); 
        }

        public void PlayAllSounds()
        {
            Debug.LogError(missingAudioServiceMessage); 
        }

        public void ChangeVolume(VolumeType volumeType, float volume)
        {
            Debug.LogError(missingAudioServiceMessage); 
        }
    }
}

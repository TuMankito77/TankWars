namespace TankWars.Runtime.Core.Audio
{
    public interface IAudioService
    {
        public void PlaySound(AudioRequest audioRequest, float volume);
        public void PlayAllSounds(); 
        public void PauseAllSounds(); 
        public void StopAllSounds();
        public void PlaySoundWithId(AudioRequest audioRequest, float volume);
        public void PauseSoundWithId(AudioRequest audioRequest);
        public void StopSoundWithId(AudioRequest audioRequest);
        public void ChangeVolume(VolumeType volumeType, float volume); 
    }
}

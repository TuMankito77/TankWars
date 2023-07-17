namespace TankWars.Runtime.Core.Audio
{
    using System.Text;
    using UnityEngine;

    public class ConsoleAudioService : IAudioService
    {
        public void PlaySound(AudioRequest audioRequest, float volume)
        {
            LogConsoleMessage(
                $"Playing sound: {audioRequest.SoundId}",
                $"Volume: {volume}",
                $"Volume type: {audioRequest.VolumeType}",
                $"Will play once: {audioRequest.WillPlayOnce}",
                $"Is spatial sound: {audioRequest.IsSpatialSound}",
                $"Sound Location: {audioRequest.SoundLocation}");
        }

        public void PlayAllSounds()
        {
            LogConsoleMessage("Playing all sounds that were paused");
        }

        public void PauseAllSounds()
        {
            LogConsoleMessage("Pausing all sounds");
        }

        public void StopAllSounds()
        {
            LogConsoleMessage("Stopping all sounds."); 
        }

        public void PlaySoundWithId(AudioRequest audioRequest, float volume)
        {
            LogConsoleMessage(
                $"Performer id: {audioRequest.PerformerId}",
                $"Playing sound: {audioRequest.SoundId}",
                $"Volume: {volume}",
                $"Volume type: {audioRequest.VolumeType}",
                $"Will play once: {audioRequest.WillPlayOnce}",
                $"Is spatial sound: {audioRequest.IsSpatialSound}",
                $"Sound Location: {audioRequest.SoundLocation}");
        }

        public void PauseSoundWithId(AudioRequest audioRequest)
        {
            LogConsoleMessage(
                $"Performer id: {audioRequest.PerformerId}",
                $"Pausing sound: {audioRequest.SoundId}",
                $"Volume type: {audioRequest.VolumeType}",
                $"Will play once: {audioRequest.WillPlayOnce}",
                $"Is spatial sound: {audioRequest.IsSpatialSound}",
                $"Sound Location: {audioRequest.SoundLocation}");
        }

        public void StopSoundWithId(AudioRequest audioRequest)
        {
            LogConsoleMessage(
                $"Performer id: {audioRequest.PerformerId}",
                $"Stopping sound: {audioRequest.SoundId}",
                $"Volume type: {audioRequest.VolumeType}",
                $"Will play once: {audioRequest.WillPlayOnce}",
                $"Is spatial sound: {audioRequest.IsSpatialSound}",
                $"Sound Location: {audioRequest.SoundLocation}");
        }

        public void ChangeVolume(VolumeType volumeType, float volume)
        {
            LogConsoleMessage(
                "Changing volume",
                $"Volume type: {volumeType}",
                $"New volume value: {volume}");
        }

        private void LogConsoleMessage(params string[] messages)
        {
            StringBuilder consoleMessage = new StringBuilder();
            consoleMessage.AppendLine($"{GetType().Name}:");
            
            foreach(string message in messages)
            {
                consoleMessage.AppendLine(message); 
            }

            Debug.Log(consoleMessage); 
        }
    }
}

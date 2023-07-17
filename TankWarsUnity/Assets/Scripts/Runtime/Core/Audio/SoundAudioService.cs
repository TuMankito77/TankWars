namespace TankWars.Runtime.Core.Audio
{
    using System.Collections.Generic;
    using TankWars.Runtime.Core.Databases;
    using TankWars.Runtime.Core.ManagerSystem;
    using TankWars.Runtime.Gameplay.ObjectManagement;
    using UnityEngine;

    public class SoundAudioService : IAudioService
    {
        private readonly Dictionary<VolumeType, List<SoundClipPlayer>> soundClipPlayersByVolumeType = new Dictionary<VolumeType, List<SoundClipPlayer>>()
        {
            {VolumeType.Music, new List<SoundClipPlayer>() },
            {VolumeType.SoundEffects, new List<SoundClipPlayer>() }
        };
        
        private SoundIdDatabase soundIdDatabase = null;
        private List<SoundClipPlayer> soundClipPlayers = null;
        private Dictionary<int, SoundClipPlayer> soundClipPlayersWithId = null;
        private Dictionary<int, bool> performerIdWasPlaying = null;

        private AudioManager AudioManager => CoreManagers.Instance.GetManager<AudioManager>(); 
        private DatabaseManager DatabaseManager => CoreManagers.Instance.GetManager<DatabaseManager>();
        private ObjectPoolManager ObjectPoolManager => CoreManagers.Instance.GetManager<ObjectPoolManager>();

        public SoundAudioService()
        {
            soundClipPlayers = new List<SoundClipPlayer>() ;
            soundIdDatabase = DatabaseManager.GetDatabase<SoundIdDatabase>();
            soundClipPlayersWithId = new Dictionary<int, SoundClipPlayer>();
            performerIdWasPlaying = new Dictionary<int, bool>(); 
            ObjectPoolManager.GeneratePoolObjects<SoundClipPlayer>(AudioManager.transform); 
        }

        public void PlaySound(AudioRequest audioRequest, float volume)
        {
            AudioClip audioClip = soundIdDatabase.GetAudioClip(audioRequest.SoundId); 
            SoundClipPlayer soundClipPlayer = ObjectPoolManager.GetPoolObject<SoundClipPlayer>();
            
            if(audioRequest.IsSpatialSound)
            {
                soundClipPlayer.transform.position = audioRequest.SoundLocation; 
            }

            void RemoveSoundClipPlayer()
            {
                ObjectPoolManager.DisablePoolObject(soundClipPlayer);
                soundClipPlayersByVolumeType[audioRequest.VolumeType].Remove(soundClipPlayer); 
                soundClipPlayers.Remove(soundClipPlayer);
                soundClipPlayer.onClipFinishedPlaying -= RemoveSoundClipPlayer; 
            }

            soundClipPlayer.onClipFinishedPlaying += RemoveSoundClipPlayer;

            soundClipPlayersByVolumeType[audioRequest.VolumeType].Add(soundClipPlayer);  
            soundClipPlayers.Add(soundClipPlayer);
            soundClipPlayer.PlayNewClip(audioClip, audioRequest.IsSpatialSound, volume, audioRequest.WillPlayOnce);
        }

        public void PlayAllSounds()
        {
            foreach (SoundClipPlayer soundClipPlayer in soundClipPlayers)
            {
                soundClipPlayer.PlayCurrentClip();
            }

            foreach (KeyValuePair<int, SoundClipPlayer> soundClipPlayerWithId in soundClipPlayersWithId)
            {
                if (performerIdWasPlaying[soundClipPlayerWithId.Key])
                {
                    performerIdWasPlaying[soundClipPlayerWithId.Key] = false;
                    soundClipPlayerWithId.Value.PlayCurrentClip();
                }
            }
        }
        
        public void PauseAllSounds()
        { 
            foreach(SoundClipPlayer soundClipPlayer in soundClipPlayers)
            {
                soundClipPlayer.PauseCurrentClip(); 
            }

            foreach(KeyValuePair<int, SoundClipPlayer> soundClipPlayerWithId in soundClipPlayersWithId)
            {
                if(soundClipPlayerWithId.Value.IsPlaying)
                {
                    performerIdWasPlaying[soundClipPlayerWithId.Key] = true;
                    soundClipPlayerWithId.Value.PauseCurrentClip(); 
                }
            }
        }

        public void StopAllSounds()
        {
            //We are looping through the list in this way becuase the list is modified when the event 
            //onClipFinishedPlaying is triggered, which happens whenever we call the StopCurrentClip. 

            while (soundClipPlayers.Count > 0)
            {
                soundClipPlayers[0].StopCurrentClip(); 
            }

            //We are looping through the dictionary in this way for the same reason as above.
            //TODO: Think of a better way to handle the removing of elements from the list and the dictionary.

            var enumerator = soundClipPlayersWithId.GetEnumerator();
            
            while(soundClipPlayersWithId.Count > 0)
            {
                enumerator.MoveNext();
                enumerator.Current.Value.StopCurrentClip(); 
            }
        }

        public void PlaySoundWithId(AudioRequest audioRequest, float volume)
        {
            SoundClipPlayer soundClipPlayer = null; 

            if(soundClipPlayersWithId.TryGetValue(audioRequest.PerformerId, out soundClipPlayer))
            {
                soundClipPlayer.PlayCurrentClip(); 
                return; 
            }

            AudioClip audioClip = soundIdDatabase.GetAudioClip(audioRequest.SoundId);
            soundClipPlayer = ObjectPoolManager.GetPoolObject<SoundClipPlayer>();

            if (audioRequest.IsSpatialSound)
            {
                soundClipPlayer.transform.position = audioRequest.SoundLocation;
            }

            void RemoveSoundClipPlayerWithPerformerId()
            {
                ObjectPoolManager.DisablePoolObject(soundClipPlayer);
                soundClipPlayersByVolumeType[audioRequest.VolumeType].Remove(soundClipPlayer); 
                soundClipPlayersWithId.Remove(audioRequest.PerformerId);
                performerIdWasPlaying.Remove(audioRequest.PerformerId); 
                soundClipPlayer.onClipFinishedPlaying -= RemoveSoundClipPlayerWithPerformerId;
            }

            soundClipPlayer.onClipFinishedPlaying += RemoveSoundClipPlayerWithPerformerId;

            soundClipPlayersByVolumeType[audioRequest.VolumeType].Add(soundClipPlayer); 
            soundClipPlayersWithId.Add(audioRequest.PerformerId, soundClipPlayer);
            performerIdWasPlaying.Add(audioRequest.PerformerId, true); 
            soundClipPlayer.PlayNewClip(audioClip, audioRequest.IsSpatialSound, volume, audioRequest.WillPlayOnce);
        }

        public void PauseSoundWithId(AudioRequest audioRequest)
        {
            if(soundClipPlayersWithId.TryGetValue(audioRequest.PerformerId, out SoundClipPlayer soundClipPlayer))
            {
                soundClipPlayer.PauseCurrentClip(); 
            }
            else
            {
                Debug.Log($"{GetType().Name}: The performer id {audioRequest.PerformerId} linked to this sound could not be found."); 
            }
        }

        public void StopSoundWithId(AudioRequest audioRequest)
        {
            if (soundClipPlayersWithId.TryGetValue(audioRequest.PerformerId, out SoundClipPlayer soundClipPlayer))
            {
                soundClipPlayer.StopCurrentClip();
            }
            else
            {
                Debug.Log($"{GetType().Name}: The performer id {audioRequest.PerformerId} linked to this sound could not be found.");
            }
        }

        public void ChangeVolume(VolumeType volumeType, float volume)
        {
            foreach(SoundClipPlayer soundClipPlayer in soundClipPlayersByVolumeType[volumeType])
            {
                soundClipPlayer.ChangeVolume(volume);
            }
        }
    }
}

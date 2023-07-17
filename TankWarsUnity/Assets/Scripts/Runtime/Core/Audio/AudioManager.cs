namespace TankWars.Runtime.Core.Audio
{
    using System;
    using TankWars.Runtime.Core.Events;
    using TankWars.Runtime.Core.ManagerSystem;
    using TankWars.Runtime.Gameplay;
    using TankWars.Runtime.Gameplay.StorableClasses;
    using UnityEngine;

    public class AudioManager : BaseManager, IEventListener
    {
        [SerializeField]
        private AudioServiceType audioServiceType = AudioServiceType.SoundAudioService; 

        private IAudioService audioService = null;
        private GameManager gameManager = null;

        public IAudioService AudioService 
        {
            get
            {
                if(audioService != null)
                {
                    return audioService; 
                }
                else
                {
                    return audioService = new NullAudioService();
                }
            }
        }

        #region Unity Methods

        private void Start()
        {
            EventManager.Instance.Register(this, typeof(AudioEvent), typeof(GameplayEvent));
        }

        private void OnDestroy()
        {
            EventManager.Instance.Unregister(this, typeof(AudioEvent), typeof(GameplayEvent)); 
        }

        #endregion

        public override void Init()
        {
            base.Init();
            gameManager = CoreManagers.Instance.GetManager<GameManager>();
            SetAudioServiceType(audioServiceType);
        }

        public void SetAudioServiceType(AudioServiceType audioServiceType)
        {
            switch (audioServiceType)
            {
                case AudioServiceType.ConsoleAudioService:
                    {
                        audioService = new ConsoleAudioService(); 
                        break;
                    }

                case AudioServiceType.SoundAudioService:
                    {
                        audioService = new SoundAudioService(); 
                        break;
                    }
            }
        }

        private void UpdateAudioServiceVolume()
        {
            GameInformation gameInformation = gameManager.GameInformation;
            audioService.ChangeVolume(VolumeType.Music, gameInformation.MusicVolume);
            audioService.ChangeVolume(VolumeType.SoundEffects, gameInformation.SoundEffectsVolume);
        }

        private float VolumeBasedOnVolumeType(VolumeType volumeType)
        {
            GameInformation gameInformation = gameManager.GameInformation;
            
            switch(volumeType)
            {
                case VolumeType.Music:
                    {
                        return gameInformation.MusicVolume; 
                    }

                case VolumeType.SoundEffects:
                    {
                        return gameInformation.SoundEffectsVolume; 
                    }

                default:
                    {
                        Debug.LogError($"{GetType()}: Unhandled type of volume."); 
                        return 0; 
                    }
            }
        }

        #region IEventListener

        public void OnEventReceived(IComparable eventType, object data)
        {
            switch(eventType)
            {
                case AudioEvent audioEvent:
                    {
                        HandleAudioEvents(audioEvent, data); 
                        break; 
                    }

                case GameplayEvent gameplayEvent:
                    {
                        HandleGameplayEvents(gameplayEvent, data); 
                        break; 
                    }

                default:
                    {
                        Debug.LogError($"{GetType().Name}-{gameObject.name}: {EventManager.UNHANDLED_EVENT_TYPE_ERROR}");
                        break; 
                    }
            }
        }

        private void HandleAudioEvents(AudioEvent audioEvent, object data)
        {
            AudioRequest audioRequest = data as AudioRequest; 

            switch(audioEvent)
            {
                case AudioEvent.PlaySound:
                    {
                        audioService.PlaySound(audioRequest, VolumeBasedOnVolumeType(audioRequest.VolumeType)); 
                        break;
                    }

                case AudioEvent.PauseAllSounds:
                    {
                        audioService.PauseAllSounds(); 
                        break; 
                    }

                case AudioEvent.StopAllSounds:
                    {
                        audioService.StopAllSounds(); 
                        break; 
                    }

                case AudioEvent.PlaySoundWithId:
                    {
                        audioService.PlaySoundWithId(audioRequest, VolumeBasedOnVolumeType(audioRequest.VolumeType)); 
                        break;
                    }

                case AudioEvent.PauseSoundWithId:
                    {
                        audioService.PauseSoundWithId(audioRequest); 
                        break;
                    }

                case AudioEvent.StopSoundWithId:
                    {
                        audioService.StopSoundWithId(audioRequest); 
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        private void HandleGameplayEvents(GameplayEvent gameplayEvent, object data)
        {
            switch(gameplayEvent)
            {
                case GameplayEvent.OnLevelRestarted:
                    {
                        audioService.StopAllSounds(); 
                        break; 
                    }

                case GameplayEvent.OnGameQuit:
                    {
                        audioService.StopAllSounds();
                        break; 
                    }

                case GameplayEvent.OnGamePaused:
                    {
                        audioService.PauseAllSounds();
                        break; 
                    }

                case GameplayEvent.OnGameUnpaused:
                    {
                        UpdateAudioServiceVolume(); 
                        audioService.PlayAllSounds(); 
                        break; 
                    }

                case GameplayEvent.OnGameOver:
                    {
                        audioService.StopAllSounds(); 
                        break; 
                    }
            }
        }

        #endregion
    }
}

namespace TankWars.Runtime.Gameplay
{
    using TankWars.Runtime.Core;
    using TankWars.Runtime.Core.Audio;
    using TankWars.Runtime.Core.Camera;
    using TankWars.Runtime.Core.Events;
    using TankWars.Runtime.Gameplay.Guns;
    using TankWars.Runtime.Core.Databases;
    using TankWars.Runtime.Gameplay.Levels;
    using TankWars.Runtime.Gameplay.Vehicles; 
    using TankWars.Runtime.Core.ManagerSystem;
    using TankWars.Runtime.Gameplay.Unlockables; 
    using TankWars.Runtime.Gameplay.ObjectManagement;
    using UnityEngine;
    using UnityEditor; 
    using System;

    public class LevelInitializer : MonoBehaviour, IEventListener
    {
        [SerializeField]
        FollowObjectCameraController followObjectCameraController = null; 

#if UNITY_EDITOR

        [SerializeField]
        private Color debugSphereColor = Color.blue; 

        [SerializeField]
        private float debugSphereRadius = 10f;

#endif
        private GameManager GameManager => CoreManagers.Instance.GetManager<GameManager>();
        private ObjectPoolManager ObjectPoolManager => CoreManagers.Instance.GetManager<ObjectPoolManager>();
        private DatabaseManager DatabaseManager => CoreManagers.Instance.GetManager<DatabaseManager>();
        private LevelIdDatabase LevelIdDatabase => DatabaseManager.GetDatabase<LevelIdDatabase>();


        #region Unity Methods 

        private void Start()
        {
            ObjectPoolManager.GeneratePoolObjects<Bullet>(); 
            TankInfoContainer tankInfoContainerSelected = GameManager.GetCurrentTankInfoContainer();
            Tank gameplayTank = Instantiate(tankInfoContainerSelected.TankPrefab, transform.position, Quaternion.identity);
            EventManager.Instance.Dispatch(GameplayEvent.NewPlayerSpawned, gameplayTank);
            PlayLevelMusic();
            EventManager.Instance.Register(this, typeof(GameplayEvent));
            LevelId currentLevelId = GameManager.CurrentLevelIdLoaded;
            LevelConfiguration levelConfiguration = LevelIdDatabase.GetLevelConfiguration(currentLevelId);
            Lightmapping.lightingSettings = levelConfiguration.LightingSettings; 

            if(followObjectCameraController)
            {
                followObjectCameraController.ChangeTarget(gameplayTank.transform); 
            }
        }

        private void OnDestroy()
        {
            ObjectPoolManager.RemoveObjectPool<Bullet>();
            EventManager.Instance.Unregister(this, typeof(GameplayEvent)); 
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = debugSphereColor; 
            Gizmos.DrawWireSphere(transform.position, debugSphereRadius); 
        }

#endif

        #endregion

        private void PlayLevelMusic()
        {
            LevelId currentLevelId = GameManager.CurrentLevelIdLoaded; 
            LevelConfiguration levelConfiguration = LevelIdDatabase.GetLevelConfiguration(currentLevelId); 

            AudioRequest audioRequest = new AudioRequest()
                .WithWillPlayOnce(false)
                .WithIsSpatialSound(false)
                .WithPerformerId(GetInstanceID())
                .WithVolumeType(VolumeType.Music)
                .WithSoundId(levelConfiguration.BackgroundMusicId);

            EventManager.Instance.Dispatch(AudioEvent.PlaySoundWithId, audioRequest); 
        }

        private void OnGameUnpaused()
        {
            
        }

        private void OnLevelRestarted()
        {
            
        }

        #region IEventListener

        public void OnEventReceived(IComparable eventType, object data)
        {
            switch(eventType)
            {
                case GameplayEvent gameplayEvent:
                    {
                        HandleGameplayEvents(gameplayEvent, data); 
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
                case GameplayEvent.OnGameUnpaused:
                    {
                        OnGameUnpaused();  
                        break; 
                    }

                case GameplayEvent.OnLevelRestarted:
                    {
                        OnLevelRestarted(); 
                        break; 
                    }

                default:
                    {
                        break; 
                    }
            }
        }

        #endregion
    }
}
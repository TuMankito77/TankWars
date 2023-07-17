namespace TankWars.Runtime.Gameplay.Guns
{
    using System;
    using TankWars.Runtime.Core;
    using TankWars.Runtime.Core.Audio;
    using TankWars.Runtime.Core.Databases;
    using TankWars.Runtime.Core.Events;
    using TankWars.Runtime.Core.ManagerSystem;
    using TankWars.Runtime.Gameplay.ObjectManagement;
    using TankWars.Runtime.Gameplay.StorableClasses;
    using UnityEngine;

    public class GunTurret : MonoBehaviour, IEventListener
    {
        public const string BULLET_SPAWN_TRANSFORM_FIELD_NAME = nameof(bulletSpawnTransform); 

        [SerializeField, Min(5)]
        private float rotationSpeed = 5f;

        [Header("In seconds:"),SerializeField]
        private float fireSpeed = 10f;

        [SerializeField]
        private Transform bulletSpawnTransform = null;

        private ObjectPoolManager ObjectPoolManager => CoreManagers.Instance.GetManager<ObjectPoolManager>();
        private GameManager GameManager => CoreManagers.Instance.GetManager<GameManager>();
        private GameInformation GameInformation => GameManager.GameInformation; 

        public float RotationSpeed 
        {
            get
            {
                return rotationSpeed; 
            }
            set
            {
                rotationSpeed = Mathf.Max(1, value); 
            }
        }
        
        public void AimTowards(Vector3 targetPosition, bool isInLocalCoordinates)
        {
            //If it's a Vector3 we assume the position is in world space coordinates,
            //but if it's a Vector2 we assume the position is in the local space coordinates of the joystick input from a controller.
            Vector3 targetPositionAtTurretHeight = isInLocalCoordinates?
                new Vector3(transform.position.x + targetPosition.x, transform.position.y, transform.position.z + targetPosition.y):
                new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
            
            Vector3 worldSpaceDirectionVector = targetPositionAtTurretHeight - transform.position;  
            Quaternion targetRotation = Quaternion.LookRotation(worldSpaceDirectionVector);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed); 
        }

        public void Fire()
        {
            Bullet bullet = ObjectPoolManager.GetPoolObject<Bullet>();
            bullet.transform.position = bulletSpawnTransform.transform.position;
            bullet.transform.rotation = Quaternion.LookRotation(transform.forward);

            AudioRequest audioRequest = new AudioRequest()
                .WithWillPlayOnce(true)
                .WithIsSpatialSound(true)
                .WithSoundId(SoundID.BULLET_SHOOT)
                .WithSoundLocation(transform.position)
                .WithVolumeType(VolumeType.SoundEffects);

            EventManager.Instance.Dispatch(AudioEvent.PlaySound, audioRequest); 
        }

        #region IEventListener

        public void OnEventReceived(IComparable eventType, object data)
        {
            switch(eventType)
            {
                case GameplayEvent gameplayEvent:
                    {
                        break; 
                    }

                default:
                    {
                        Debug.LogError(EventManager.UNHANDLED_EVENT_TYPE_ERROR); 
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

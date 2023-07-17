namespace TankWars.Runtime.Gameplay.Enemy
{
    using TankWars.Runtime.Gameplay;
    using TankWars.Runtime.Gameplay.Guns;
    using TankWars.Runtime.Core.Events;
    using TankWars.Runtime.Core.Databases;
    using UnityEngine;
    using System;
    using EnemyTypeSettings = Core.Databases.EnemyTypeDatabase.EnemyTypeSettings;
    using TankWars.Runtime.Core.ManagerSystem;

    public abstract class EnemyGameplayInformation : MonoBehaviour, IEventListener
    {
        private const float MIN_HEALTH_AMOUNT = 0;

        //TODO: Use this action to update the health of the enemy on the screen
        public Action<float> onEnemyHealthChange = null; 

        [SerializeField]
        private EnemyType enemyType = EnemyType.NONE;

        private float health = 30f;
        
        protected EnemyTypeSettings enemyTypeSettings = default(EnemyTypeSettings);
        protected abstract int GOWithColliderForDetectingImpactIntanceId { get; }

        public float MaxHealthAmount => enemyTypeSettings.healthAmount;

        #region Unity Methods

        protected virtual void Start()
        {
            DatabaseManager databaseManager = CoreManagers.Instance.GetManager<DatabaseManager>();
            EnemyTypeDatabase enemyTypeDatabase = databaseManager.GetDatabase<EnemyTypeDatabase>(); 
            enemyTypeSettings = enemyTypeDatabase.GetEnemyTypeSettings(enemyType);
            health = enemyTypeSettings.healthAmount;
            EventManager.Instance.Register(this, typeof(GameplayEvent)); 
        }

        protected virtual void OnDestroy()
        {
            EventManager.Instance.Unregister(this, typeof(GameplayEvent));
        }

        #endregion

        private void ChangeHealthAmount(float amount)
        {
            health = Mathf.Clamp(health + amount, MIN_HEALTH_AMOUNT, enemyTypeSettings.healthAmount);
            onEnemyHealthChange?.Invoke(health); 
        }

        #region IEventListener

        public void OnEventReceived(IComparable eventType, object data)
        {
            switch (eventType)
            {
                case GameplayEvent gameplayEvent:
                    {
                        HandleGameplayEvents(gameplayEvent, data);
                        break;
                    }

                default:
                    {
                        Debug.LogError($"{this}-{gameObject.name}: The class attached to this GO has recieved an unhandled type of event.");
                        break;
                    }
            }
        }

        private void HandleGameplayEvents(GameplayEvent gameplayEvent, object data)
        {
            switch (gameplayEvent)
            {
                case GameplayEvent.EntityDamaged:
                    {
                        Bullet bullet = (Bullet)data;
 
                        if (GOWithColliderForDetectingImpactIntanceId != bullet.DamagedGameObjectInstanceId)
                        {
                            return;
                        }

                        ChangeHealthAmount(-bullet.DamageAmount);
                        break;
                    }
            }
        }

        #endregion
    }
}

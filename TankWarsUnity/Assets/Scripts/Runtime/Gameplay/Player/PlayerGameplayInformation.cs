namespace TankWars.Runtime.Gameplay.Player
{
    using TankWars.Runtime.Gameplay;
    using TankWars.Runtime.Core.Events;
    using UnityEngine;
    using System;
    using TankWars.Runtime.Gameplay.Guns;
    using TankWars.Runtime.Gameplay.Unlockables;
    using TankWars.Runtime.Gameplay.Vehicles;
    using TankWars.Runtime.Gameplay.PowerUps;
    using TankWars.Runtime.Core.Tools.Time;

    public class PlayerGameplayInformation : IEventListener, IDisposable
    {
        public const string PLAYER_TAG = "Player"; 
        public const float MIN_HEALTH_AMOUNT = 0f;
        private const float MAX_HEALTH_AMOUNT = 100f;
        private const int MIN_POINTS_AMOUNT = 0;
        private const int INVALID_TANK_INSTANCE = -1;

        private int linkedTankInstance = INVALID_TANK_INSTANCE;
        private Tank tank = null;
        private TankInfoContainer tankInfoContainer = null;
        private Timer powerUpTimer = null;
        private bool isPlayerAlive = true;

        public float Health { get; private set; } = MAX_HEALTH_AMOUNT;
        public int Points { get; private set; } = MIN_POINTS_AMOUNT;

        public PlayerGameplayInformation(Tank sourceTank, TankInfoContainer sourceTankInfoContainer)
        {
            linkedTankInstance = sourceTank.gameObject.GetInstanceID();
            tank = sourceTank;
            tankInfoContainer = sourceTankInfoContainer;
            tank.SetTankSpeed(tankInfoContainer.MovementSpeed);
            tank.SetTankAimingSpeed(tankInfoContainer.AimingSpeed);
            EventManager.Instance.Register(this, typeof(GameplayEvent), typeof(PowerUpType));
        }

        private void ChangeHealth(float amount)
        {
            Health = Mathf.Clamp(Health + amount, MIN_HEALTH_AMOUNT, MAX_HEALTH_AMOUNT);
            EventManager.Instance.Dispatch(PlayerGameplayEvents.HealthChange, this);

            if(Health <= 0 && isPlayerAlive)
            {
                isPlayerAlive = false; 
                EventManager.Instance.Dispatch(PlayerGameplayEvents.HasDied, this); 
            }
        }

        private void ChangePoints(int amount)
        {
            Points = Mathf.Max(Points + amount, MIN_POINTS_AMOUNT);
            EventManager.Instance.Dispatch(PlayerGameplayEvents.PointsChange, this);
        }

        private void OnPowerUpTimerCompleted()
        {
            tank.SetTankSpeed(tankInfoContainer.MovementSpeed);
            tank.SetTankAimingSpeed(tankInfoContainer.AimingSpeed);
            powerUpTimer = null;
        }

        #region IEventListener

        public void OnEventReceived(IComparable eventType, object data)
        {
            switch (eventType)
            {
                case GameplayEvent gameplayEvent:
                    {
                        HandleGameplayEvent(gameplayEvent, data);
                        break;
                    }

                case PowerUpType powerUpType:
                    {
                        HandlePowerupEvents(powerUpType, data);
                        break;
                    }

                default:
                    {
                        Debug.LogError($"{this}: The class recieved an unhandled type of event.");
                        break;
                    }
            }
        }

        private void HandleGameplayEvent(GameplayEvent gameplayEvent, object data)
        {
            switch (gameplayEvent)
            {
                case GameplayEvent.EntityDamaged:
                    {
                        Bullet bullet = (Bullet)data;

                        if (tank.gameObject.GetInstanceID() != bullet.DamagedGameObjectInstanceId)
                        {
                            return;
                        }

                        ChangeHealth(-bullet.DamageAmount);
                        break;
                    }
            }
        }

        private void HandlePowerupEvents(PowerUpType powerUpType, object data)
        {
            BasePowerUp powerUp = (BasePowerUp)data;

            if (powerUp.TakerGameObjectInstanceId != linkedTankInstance)
            {
                return;
            }

            if (powerUpType == PowerUpType.Healing)
            {
                ChangeHealth(powerUp.BoostAmount);
                return;
            }

            if (powerUpTimer != null)
            {
                powerUpTimer.onTimerCompleted -= OnPowerUpTimerCompleted;
                OnPowerUpTimerCompleted();
            }

            powerUpTimer = new Timer(powerUp.Duration, true);
            powerUpTimer.onTimerCompleted += OnPowerUpTimerCompleted;

            switch (powerUpType)
            {
                case PowerUpType.MovementSpeed:
                    {
                        tank.SetTankSpeed(tankInfoContainer.MovementSpeed + powerUp.BoostAmount);
                        break;
                    }

                case PowerUpType.AimingSpeed:
                    {
                        tank.SetTankAimingSpeed(tankInfoContainer.AimingSpeed + powerUp.BoostAmount);
                        break;
                    }

                case PowerUpType.Fire:
                    {
                        break;
                    }

                case PowerUpType.None:
                    {
                        Debug.Log($"{GetType()}-{tank.gameObject.name}:The power-up taken has no effect.");
                        break;
                    }

                default:
                    {
                        Debug.LogError($"{GetType()}-{tank.gameObject.name}: The power-up type is an unhandled case!");
                        break;
                    }
            }

            powerUpTimer.Start();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            EventManager.Instance.Unregister(this, typeof(GameplayEvent), typeof(PowerUpType));
        }

        #endregion
    }
}

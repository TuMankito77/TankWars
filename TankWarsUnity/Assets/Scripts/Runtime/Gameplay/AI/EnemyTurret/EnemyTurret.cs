namespace TankWars.Runtime.Gameplay.AI.EnemyTurret
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using TankWars.Runtime.Core.Events;
    using TankWars.Runtime.Gameplay.Guns;
    using TankWars.Runtime.Gameplay.Vehicles;
    using UnityEngine;

    public class EnemyTurret : BaseEnemy, IEventListener
    {
        public event Action onTargetFound = null;

        [SerializeField]
        private GunTurret gunTurret = null;

        [SerializeField, Min(1f)]
        private float fireDelay = 1f;

        private Coroutine shootTargetCoroutine = null;
        private List<Tank> playerTanks = new List<Tank>();
        private List<GameObject> targetsInSight = new List<GameObject>();
        private bool canSeeTarget => targetsInSight.Count > 0;
        private bool isGamePaused = false;

        #region Unity Methods

        private void Start()
        {
            EventManager.Instance.Register(this, typeof(GameplayEvent));
        }

        private void OnDestroy()
        {
            EventManager.Instance.Unregister(this, typeof(GameplayEvent));
        }

        private void Update()
        {
            if (playerTanks.Count <= 0 || isGamePaused)
            {
                return;
            }

            foreach (Tank playerTank in playerTanks)
            {
                float distanceToTarget = Vector3.Distance(transform.position, playerTank.transform.position);

                if (distanceToTarget < attackingDistance && !targetsInSight.Contains(playerTank.gameObject))
                {
                    targetsInSight.Add(playerTank.gameObject);
                }
                else if (distanceToTarget >= attackingDistance && targetsInSight.Contains(playerTank.gameObject))
                {
                    targetsInSight.Remove(playerTank.gameObject);
                }
            }

            if (canSeeTarget && shootTargetCoroutine == null)
            {
                int randomTarget = UnityEngine.Random.Range(0, targetsInSight.Count);
                shootTargetCoroutine = StartCoroutine(ShootTarget(targetsInSight[randomTarget].transform));
            }
        }

        #endregion

        private IEnumerator ShootTarget(Transform currentTargetTransform)
        {
            float lastShotTimeElapsed = 0;

            while (canSeeTarget)
            {
                gunTurret.AimTowards(currentTargetTransform.position, false);
                lastShotTimeElapsed += Time.deltaTime;

                if (lastShotTimeElapsed > fireDelay)
                {
                    gunTurret.Fire();
                    lastShotTimeElapsed = 0;
                }

                yield return null;
            }

            shootTargetCoroutine = null;
        }

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
                        Debug.LogError($"{GetType()}-{gameObject.name}:{EventManager.UNHANDLED_EVENT_TYPE_ERROR}");
                        break;
                    }
            }
        }

        private void HandleGameplayEvents(GameplayEvent gameplayEvent, object data)
        {
            switch (gameplayEvent)
            {
                case GameplayEvent.NewPlayerSpawned:
                    {
                        Tank playerTank = data as Tank;

                        if (playerTanks.Contains(playerTank))
                        {
                            break;
                        }

                        playerTanks.Add(playerTank);
                        break;
                    }

                case GameplayEvent.OnGamePaused:
                    {
                        isGamePaused = true;

                        if (shootTargetCoroutine != null)
                        {
                            StopCoroutine(shootTargetCoroutine); 
                        }

                        break; 
                    }

                case GameplayEvent.OnGameUnpaused:
                    {
                        isGamePaused = false; 
                        break; 
                    }

                case GameplayEvent.OnGameOver:
                case GameplayEvent.OnGameQuit:
                case GameplayEvent.OnLevelRestarted:
                    {
                        playerTanks.Clear();
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }
    }
}

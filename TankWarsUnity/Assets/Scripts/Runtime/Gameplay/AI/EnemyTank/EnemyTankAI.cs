namespace TankWars.Runtime.Gameplay.AI.EnemyTank
{
    using TankWars.Runtime.Gameplay.Vehicles;
    using TankWars.Runtime.Core.AI.FiniteStateMachine;
    using TankWars.Runtime.Core.Tools.Time;
    using UnityEngine.AI;
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using TankWars.Runtime.Core.Events;
    using System;
    using TankWars.Runtime.Gameplay.Enemy;

    public class EnemyTankAI : BaseEnemy, IEventListener
    {
        [SerializeField, Min(0.1f)]
        private float pathFolowingAccuracy = 0.1f;

        [SerializeField]
        private float pursuingDistance = 5f;

        [SerializeField]
        private Transform[] patrolPoints = null;

        [SerializeField]
        private Tank tankController = null;

        [SerializeField, Range(1, 3)]
        private float fireDelay = 3f;

        [SerializeField]
        private Collider collider = null;

        [SerializeField]
        private EnemyGameplayInformation enemyGameplayInformation = null; 

        private StateMachine<EnemyTankAI> stateMachine = null;
        private Coroutine followPathCoroutine = null;
        private Queue<Vector3> destinations = null;
        private Timer fireTimer = null;
        private bool canEnemyFire = false;
        private List<Tank> playerTanks = new List<Tank>(); 
        private Vector3[] debugWaypoints = null;

        public DeadState DeadState { get; private set; } = null; 
        public PatrolState PatrolState { get; private set; } = null;
        public PursueState PursueState { get; private set; } = null;
        public AttackState AttackState { get; private set; } = null;
        public InvestigateState InvestigateState { get; private set; } = null;
        public Transform Target { get; private set; } = null;
        public bool IsTargetInSight { get; private set; } = false;
        public Vector3 LastTargetPosition { get; private set; } = Vector3.zero;
        public float AttackingDistance => attackingDistance;
        public Coroutine FollowPathCoroutinge => followPathCoroutine;
        public Collider Collider => collider; 

        public float DistanceToTarget
        {
            get
            {
                if(Target == null)
                {
                    return -1; 
                }
                else
                {
                    return Vector3.Distance(Target.transform.position, tankController.transform.position);
                }
            }
        }

        #region Unity Methods

        private void Start()
        {
            canEnemyFire = true; 
            fireTimer = new Timer(fireDelay, true);
            fireTimer.onTimerCompleted += OnFireTimerComplete;
            enemyGameplayInformation.onEnemyHealthChange += OnDamageReceived; 
            destinations = new Queue<Vector3>();
            stateMachine = new StateMachine<EnemyTankAI>();
            PatrolState = new PatrolState(this, stateMachine, patrolPoints);
            PursueState = new PursueState(this, stateMachine);
            AttackState = new AttackState(this, stateMachine);
            DeadState = new DeadState(this, stateMachine); 
            InvestigateState = new InvestigateState(this, stateMachine);
            stateMachine.Initialize(PatrolState);
            EventManager.Instance.Register(this, typeof(GameplayEvent));
        }

        private void OnDestroy()
        {
            fireTimer.onTimerCompleted -= OnFireTimerComplete;
            EventManager.Instance.Unregister(this, typeof(GameplayEvent)); 
        }

        private void Update()
        {
            stateMachine.CurrentState.LogicUpdate();

            foreach(Tank playerTank in playerTanks)
            {
                float distanceToTarget = Vector3.Distance(playerTank.transform.position, tankController.transform.position);
            
                if(Target == null && distanceToTarget <= pursuingDistance)
                {
                    Target = playerTank.transform;
                    IsTargetInSight = true;
                }
                else if(Target == playerTank.transform && distanceToTarget > pursuingDistance)
                {
                    LastTargetPosition = playerTank.transform.position;
                    IsTargetInSight = false;
                    Target = null;
                }
            }
        }

        private void FixedUpdate()
        {
            stateMachine.CurrentState.PhysicsUpdate();
        }

        private void OnValidate()
        {
            pursuingDistance = Mathf.Max(pursuingDistance, attackingDistance); 
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            
            if (!drawDebugSpeheres) return; 

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, pursuingDistance);

            if (debugWaypoints == null)
            {
                return;
            }

            foreach (Vector3 waypoint in debugWaypoints)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(waypoint, 0.5f);
            }
        }

        #endregion

        public void FireProjectile()
        {
            if(!canEnemyFire)
            {
                return;
            }

            canEnemyFire = false; 
            tankController.Fire();
            fireTimer.Restart(); 
        }

        public void AimTurretTowardsTarget()
        {
            tankController.AimTurret(Target.position, false);
        }

        public void SetNewPath(Vector3 destination)
        {
            if (followPathCoroutine != null)
            {
                StopCoroutine(followPathCoroutine);
                destinations.Clear();
            }

            NavMeshPath newPath = new NavMeshPath();
            NavMesh.CalculatePath(tankController.transform.position, destination, NavMesh.AllAreas, newPath);
            followPathCoroutine = StartCoroutine(FollowPath(newPath));
        }

        public void AppendToCurrentPath(Vector3 destination)
        {
            NavMeshPath newPath = new NavMeshPath();
            NavMesh.CalculatePath(tankController.transform.position, destination, NavMesh.AllAreas, newPath);

            if (followPathCoroutine == null)
            {
                followPathCoroutine = StartCoroutine(FollowPath(newPath));
                return;
            }

            destinations.Enqueue(destination);
        }

        public void StopFollowingPath()
        {
            if (followPathCoroutine == null)
            {
                return;
            }

            StopCoroutine(followPathCoroutine);
            followPathCoroutine = null;
            destinations.Clear();
        }

        private IEnumerator FollowPath(NavMeshPath navMeshPath)
        {
            debugWaypoints = navMeshPath.corners;

            foreach (Vector3 waypoint in navMeshPath.corners)
            {
                Vector3 waypointAtTankHeihgt = new Vector3(waypoint.x, tankController.transform.position.y, waypoint.z);
                float distanceToWaypoint = Vector3.Distance(tankController.transform.position, waypointAtTankHeihgt);

                while (distanceToWaypoint > pathFolowingAccuracy)
                {
                    tankController.MoveTowards(waypoint, false);
                    waypointAtTankHeihgt.y = tankController.transform.position.y; 
                    distanceToWaypoint = Vector3.Distance(tankController.transform.position, waypointAtTankHeihgt);
                    yield return new WaitForFixedUpdate();
                }
            }

            if (destinations.Count > 0)
            {
                NavMeshPath nextPath = new NavMeshPath();
                Vector3 nextDestination = destinations.Dequeue();
                NavMesh.CalculatePath(tankController.transform.position, nextDestination, NavMesh.AllAreas, nextPath);
                followPathCoroutine = StartCoroutine(FollowPath(nextPath));
                debugWaypoints = nextPath.corners;
            }
            else
            {
                followPathCoroutine = null;
                debugWaypoints = null;
            }
        }

        private void OnFireTimerComplete()
        {
            canEnemyFire = true; 
        }

        private void OnDamageReceived(float health)
        {
            if(health == 0)
            {
                stateMachine.ChangeState(DeadState); 
            }    
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
                        Debug.LogError($"{GetType()}-{EventManager.UNHANDLED_EVENT_TYPE_ERROR}");
                        break;
                    }
            }
        }

        private void HandleGameplayEvents(GameplayEvent gameplayEvent, object data)
        {
            switch(gameplayEvent)
            {
                case GameplayEvent.NewPlayerSpawned:
                    {
                        if(!(data is Tank playerTank))
                        {
                            break; 
                        }

                        if(!playerTanks.Contains(playerTank))
                        {
                            playerTanks.Add(playerTank); 
                        }

                        break; 
                    }

                case GameplayEvent.OnGameQuit:
                case GameplayEvent.OnGameOver:
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

        #endregion
    }
}

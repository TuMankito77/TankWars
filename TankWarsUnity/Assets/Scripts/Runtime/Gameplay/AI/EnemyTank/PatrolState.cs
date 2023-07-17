namespace TankWars.Runtime.Gameplay.AI.EnemyTank
{
    using TankWars.Runtime.Core.AI.FiniteStateMachine;
    using UnityEngine;

    public class PatrolState : State<EnemyTankAI>
    {
        private Transform[] patrolPoints = null;
        private Transform currentPatrolPoint = null;
        private int currentPatrolPointIndex = 0;

        public PatrolState(EnemyTankAI sourceEntityToControl, StateMachine<EnemyTankAI> sourceStateMachine, Transform[] sourcePatrolPoints)
            : base(sourceEntityToControl, sourceStateMachine)
        {
            patrolPoints = sourcePatrolPoints;
        }

        public override void Enter()
        {
            if (patrolPoints.Length <= 0)
            {
                return;
            }

            currentPatrolPoint = patrolPoints[currentPatrolPointIndex];
            entityController.SetNewPath(currentPatrolPoint.position);
        }

        public override void Exit()
        {

        }

        public override void LogicUpdate()
        {
            if (entityController.IsTargetInSight)
            {
                stateMachine.ChangeState(entityController.PursueState);
            }

            if (patrolPoints.Length <= 0)
            {
                return;
            }

            if (entityController.FollowPathCoroutinge == null)
            {
                currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
                currentPatrolPoint = patrolPoints[currentPatrolPointIndex];
                entityController.SetNewPath(currentPatrolPoint.position);
            }
        }

        public override void PhysicsUpdate()
        {

        }
    }
}

namespace TankWars.Runtime.Gameplay.AI.EnemyTank
{
    using TankWars.Runtime.Core.AI.FiniteStateMachine;
    using UnityEngine;

    public class PursueState : State<EnemyTankAI>
    {
        private const float FOLLOW_TARGET_ACCURACY = 1.5f;

        private Vector3 targetLastPosition = default(Vector3);

        public PursueState(EnemyTankAI sourceEntityToControl, StateMachine<EnemyTankAI> sourceStateMachine)
            : base(sourceEntityToControl, sourceStateMachine)
        {

        }

        public override void Enter()
        {
            targetLastPosition = entityController.Target.transform.position;
            entityController.SetNewPath(targetLastPosition);
        }

        public override void Exit()
        {
            entityController.StopFollowingPath(); 
        }

        public override void LogicUpdate()
        {
            if (!entityController.IsTargetInSight)
            {
                stateMachine.ChangeState(entityController.InvestigateState);
                return;
            }

            if (entityController.DistanceToTarget < entityController.AttackingDistance)
            {
                entityController.StopFollowingPath();
                stateMachine.ChangeState(entityController.AttackState); 
            }

            float targetToLastPositionDistance = Vector3.Distance(entityController.Target.position, targetLastPosition);

            if (targetToLastPositionDistance > FOLLOW_TARGET_ACCURACY)
            {
                targetLastPosition = entityController.Target.transform.position;
                entityController.AppendToCurrentPath(targetLastPosition);
            }
        }

        public override void PhysicsUpdate()
        {

        }
    }
}

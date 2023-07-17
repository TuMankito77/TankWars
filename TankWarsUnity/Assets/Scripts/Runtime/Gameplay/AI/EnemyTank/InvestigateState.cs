namespace TankWars.Runtime.Gameplay.AI.EnemyTank
{
    using TankWars.Runtime.Core.AI.FiniteStateMachine;

    public class InvestigateState : State<EnemyTankAI>
    {
        public InvestigateState(EnemyTankAI sourceEntityToControl, StateMachine<EnemyTankAI> sourceStateMachine) : base(sourceEntityToControl, sourceStateMachine)
        {
        }

        public override void Enter()
        {
            entityController.SetNewPath(entityController.LastTargetPosition); 
        }

        public override void Exit()
        {
            entityController.StopFollowingPath(); 
        }

        public override void LogicUpdate()
        {
            if(entityController.IsTargetInSight)
            {
                stateMachine.ChangeState(entityController.PursueState); 
            }

            if(entityController.FollowPathCoroutinge == null)
            {
                stateMachine.ChangeState(entityController.PatrolState); 
            }
        }

        public override void PhysicsUpdate()
        {
             
        }
    }
}

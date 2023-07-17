namespace TankWars.Runtime.Gameplay.AI.EnemyTank
{
    using TankWars.Runtime.Core.AI.FiniteStateMachine;

    public class AttackState : State<EnemyTankAI>
    {
        public AttackState(EnemyTankAI sourceEntityToControl, StateMachine<EnemyTankAI> sourceStateMachine) : base(sourceEntityToControl, sourceStateMachine)
        {
        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            
        }

        public override void LogicUpdate()
        {
            if (entityController.DistanceToTarget > entityController.AttackingDistance)
            {
                stateMachine.ChangeState(entityController.PursueState); 
            }
            
            entityController.FireProjectile();
            entityController.AimTurretTowardsTarget(); 
        }

        public override void PhysicsUpdate()
        {
            
        }
    }

}

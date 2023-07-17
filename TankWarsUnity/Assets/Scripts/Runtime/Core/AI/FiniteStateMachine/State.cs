namespace TankWars.Runtime.Core.AI.FiniteStateMachine
{
    public abstract class State<T>
    {
        protected T entityController = default(T);
        protected StateMachine<T> stateMachine = null;

        public State(T sourceEntityToControl, StateMachine<T> sourceStateMachine)
        {
            entityController = sourceEntityToControl;
            stateMachine = sourceStateMachine;
        }

        public abstract void Enter();

        public abstract void LogicUpdate();

        public abstract void PhysicsUpdate();

        public abstract void Exit();
    }

}

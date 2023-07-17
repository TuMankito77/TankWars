namespace TankWars.Runtime.Core.AI.FiniteStateMachine
{
    public class StateMachine<T>
    {
        public State<T> CurrentState { get; private set; } = null;

        public void Initialize(State<T> beginningState)
        {
            CurrentState = beginningState;
            CurrentState.Enter();
        }

        public void ChangeState(State<T> newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}

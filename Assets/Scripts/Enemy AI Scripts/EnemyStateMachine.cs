public interface IEnemyState
{
    void Enter();
    void Update();
    void FixedUpdate();
    void Exit();
}

public interface IEnemyStateMachine
{
    void ChangeState(IEnemyState newState);
}

public class EnemyStateMachine : IEnemyStateMachine
{
    private IEnemyState currentState;

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.Enter();
        }
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }

    public void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.FixedUpdate();
        }
    }
}

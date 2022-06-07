public class GameLoopState : IState
{
    private GameStateMachine _stateMachine;
    private GameStarter _gameStarter;

    public GameLoopState(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Exit()
    {
    }

    public void Enter()
    {
    }
}

public class IslandState : IState
{
    private GameStateMachine _stateMachine;
    private CameraFollower _cameraFollower;
    public IslandState(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Exit()
    {
    }

    public void Enter()
    {
    }
}
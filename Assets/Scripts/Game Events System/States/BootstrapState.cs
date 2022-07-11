using UnityEngine;

public class BootstrapState : IState
{
    private const string Initial = "Demo 2";
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;

    public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader)
    {
        Economy economy = new Economy();
        _stateMachine = stateMachine;
        _sceneLoader = sceneLoader;
        Services.Container.RegisterSingle(economy);
    }

    public void Enter()
    {
        EnterLoadLevel();
    }

    public void Exit()
    {
    }

    private void EnterLoadLevel()
    {
        _stateMachine.Enter<LoadGameState, string>(Initial);
    }
}

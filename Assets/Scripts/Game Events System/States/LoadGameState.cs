using UnityEngine;

public class LoadGameState : IPayloadedState<string>
{
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;

    public LoadGameState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
    {
        _stateMachine = gameStateMachine;
        _sceneLoader = sceneLoader;
        _loadingCurtain = loadingCurtain;
    }

    public void Enter(string sceneName)
    {
        _loadingCurtain.Show();
        _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() =>
      _loadingCurtain.Hide();

    private void OnLoaded()
    {
        Debug.Log("Scene has loaded");
        _stateMachine.Enter<GameLoopState>();        
    }
}

public class LoadLevelState : IState
{
    private readonly GameStateMachine _stateMachine;
    private readonly LoadingCurtain _loadingCurtain;

    public LoadLevelState(GameStateMachine gameStateMachine, LoadingCurtain loadingCurtain)
    {
        _stateMachine = gameStateMachine;
        _loadingCurtain = loadingCurtain;
    }

    public void Enter()
    {
        _loadingCurtain.Show();
        _stateMachine.Enter<GameLoopState>();        
    }

    public void Exit() =>
      _loadingCurtain.Hide();
}

public class ReloadLevel : IState
{
    private readonly GameStateMachine _stateMachine;
    private readonly LoadingCurtain _loadingCurtain;

    public ReloadLevel(GameStateMachine gameStateMachine, LoadingCurtain loadingCurtain)
    {
        _stateMachine = gameStateMachine;
        _loadingCurtain = loadingCurtain;
    }

    public void Enter()
    {
        _loadingCurtain.Show();
        _stateMachine.Enter<GameLoopState>();
    }

    public void Exit() =>
      _loadingCurtain.Hide();
}

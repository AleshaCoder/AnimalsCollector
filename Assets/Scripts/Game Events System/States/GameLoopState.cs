using UnityEngine;

public class GameLoopState : IState
{
    private GameStateMachine _stateMachine;
    private GameStarter _gameStarter;
    private CombineCollector _combineCollector;
    private LevelsPool _levelsPool;
    private LevelTrigger _levelTrigger;

    public GameLoopState(GameStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public void Exit()
    {
        _combineCollector.OnCollect -= _levelsPool.CurrentLevel.Decrease;
        _levelsPool.CurrentLevel.OnTarget -= _levelTrigger.SwitchOn;
        _levelTrigger.OnTrigger -= _stateMachine.Enter<LoadLevelState>;
    }

    public void Enter()
    {
        Debug.Log("Game loop");
        _combineCollector = Services.Container.Single<CombineCollector>();
        _levelsPool = Services.Container.Single<LevelsPool>();
        _levelTrigger = Services.Container.Single<LevelTrigger>();
        
        _combineCollector.OnCollect += _levelsPool.CurrentLevel.Decrease;
        _levelsPool.CurrentLevel.OnTarget += _levelTrigger.SwitchOn;
        _levelTrigger.OnTrigger += _stateMachine.Enter<LoadLevelState>;
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
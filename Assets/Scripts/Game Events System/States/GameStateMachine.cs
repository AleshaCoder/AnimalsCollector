using System;
using System.Collections.Generic;

public class GameStateMachine
{
    private Dictionary<Type, IExitableState> _states;
    private IExitableState _activeState;

    public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
    {
        _states = new Dictionary<Type, IExitableState>
        {
            [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader),
            [typeof(LoadGameState)] = new LoadGameState(this, sceneLoader, loadingCurtain),
            [typeof(GameLoopState)] = new GameLoopState(this),
            [typeof(IslandState)] = new IslandState(this),
            [typeof(LoadLevelState)] = new LoadLevelState(this, loadingCurtain),
            [typeof(ReloadLevel)] = new ReloadLevel(this, loadingCurtain)
        };
    }

    public void Enter<TState>() where TState : class, IState
    {
        IState state = ChangeState<TState>();
        state.Enter();
    }

    public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
    {
        TState state = ChangeState<TState>();
        state.Enter(payload);
    }

    private TState ChangeState<TState>() where TState : class, IExitableState
    {
        _activeState?.Exit();

        TState state = GetState<TState>();
        _activeState = state;

        return state;
    }

    private TState GetState<TState>() where TState : class, IExitableState =>
      _states[typeof(TState)] as TState;
}

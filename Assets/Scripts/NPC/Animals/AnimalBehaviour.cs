using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBehaviour : MonoBehaviour
{
    public enum Behaviour
    {
        Idle,
        Panic,
        Caught
    }

    [Header("States")]
    [SerializeField] private FreeWalkingState _freeWalking;
    [SerializeField] private RunAwayState _runAwayState;
    [SerializeField] private IdleState _idleState;
    [Space] [SerializeField] private HeroDetector _heroDetector;
    [SerializeField] private MonoBehaviour _pickable;
    [SerializeField] private Animator _animator;

    private Dictionary<Type, State> _states;
    private State _activeState;
    private Behaviour _currentBehaviour;

    private IPickable Pickable => _pickable as IPickable;

    public Behaviour CurrentBehaviour => _currentBehaviour;

    public delegate void Exit();

    public void InitStates()
    {
        _states = new Dictionary<Type, State>()
        {
            [typeof(FreeWalkingState)] = _freeWalking,
            [typeof(RunAwayState)] = _runAwayState,
            [typeof(IdleState)] = _idleState
        };

        foreach (var item in _states)
            if (item.Value is StateWithAnimator)
                ((StateWithAnimator)item.Value).Init(_animator);
    }

    public void Enter<TState>(Exit onExit = null) where TState : State
    {
        State state = ChangeState<TState>();
        if (onExit != null)
            state.OnExit += () => onExit();
        state.Enter();
    }

    private TState ChangeState<TState>() where TState : State
    {
        _activeState?.Exit();

        TState state = GetState<TState>();
        _activeState = state;

        return state;
    }

    private TState GetState<TState>() where TState : State =>
      _states[typeof(TState)] as TState;

    private void StartUsualState()
    {
        var randNumber = UnityEngine.Random.Range(0, 2);

        if (randNumber == 0)
            Enter<FreeWalkingState>(onExit: StartUsualState);
        else
            Enter<IdleState>(onExit: StartUsualState);

        _currentBehaviour = Behaviour.Idle;
    }

    private void StartPanicState()
    {
        StartRunning();
        _currentBehaviour = Behaviour.Panic;
    }

    private void StartRunning()
    {
        Enter<RunAwayState>(onExit: StartUsualState);
    }

    private void OnEnable()
    {
        InitStates();
        Pickable.OnPick += () =>
        {
            _heroDetector.SwitchOff();
            Enter<IdleState>();
        };
        _heroDetector.OnLoss += StartUsualState;
        _heroDetector.OnDetection += StartPanicState;
        Services.Container.Single<LevelLoader>().OnStartLevelChanged += SwitchOff;
        Services.Container.Single<LevelLoader>().OnLevelChanged += Init;
    }

    private void SwitchOff()
    {
        Enter<IdleState>();
        enabled = false;
    }

    private void OnDisable()
    {
        Pickable.OnPick -= () =>
        {
            _heroDetector.SwitchOff();
            Enter<IdleState>();
        };
        _heroDetector.OnLoss -= StartUsualState;
        _heroDetector.OnDetection -= StartPanicState;
        _heroDetector.SwitchOff();
        Services.Container.Single<LevelLoader>().OnStartLevelChanged -= SwitchOff;
        Services.Container.Single<LevelLoader>().OnLevelChanged -= Init;
    }

    private void Init(Level level)
    {        
        StartUsualState();
        _heroDetector.SwitchOn();
    }

    private void OnValidate()
    {
        if (_pickable is not IPickable)
        {
            _pickable = null;
            Debug.LogError(_pickable.name + " needs to implement " + nameof(IPickable));
        }
    }
}

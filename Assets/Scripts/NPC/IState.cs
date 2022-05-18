using System;
using UnityEngine;

public interface IState
{
    void Enter();
    void Exit();
}

public interface IRunnableState : IState
{
    bool Finished
    {
        get;
    }
    void Run();
}

public abstract class StateWithFinish : MonoBehaviour, IState
{
    public Action OnFinished;

    public abstract void Enter();

    public abstract void Exit();
}
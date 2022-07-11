using System;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public bool Finished { get; protected set; }
    public Action OnExit;

    public virtual void Enter()
    {
        Finished = false;
    }

    public virtual void Exit()
    {
        OnExit = null;
        Finished = true;
    }
}

public abstract class StateWithAnimator : State
{
    public abstract void Init(Animator animator);
}
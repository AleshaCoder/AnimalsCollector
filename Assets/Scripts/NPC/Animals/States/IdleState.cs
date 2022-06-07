using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class IdleState : State
{
    public override void Enter()
    {
        base.Enter();
        End();
    }

    private async void End()
    {
        await Task.Delay(10000);
        OnExit?.Invoke();
        OnExit = null;
    }
}

using System.Threading.Tasks;
using UnityEngine;

public class IdleState : StateWithAnimator
{
    private Animator _animator;

    public override void Enter()
    {
        base.Enter();
        _animator.Play(AnimatorChick.States.Idle);
        End();
    }

    public override void Init(Animator animator) => _animator = animator;

    private async void End()
    {
        await Task.Delay(10000);
        OnExit?.Invoke();
        OnExit = null;
    }
}

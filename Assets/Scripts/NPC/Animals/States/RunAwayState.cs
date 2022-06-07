using DG.Tweening;
using System.Collections;
using UnityEngine;

public class RunAwayState : State
{
    [SerializeField] private Transform _dangerousTransform;
    [SerializeField] private WalkableArea _walkableArea;
    [SerializeField] private float _speed;

    private Tweener _tweener;
    private IEnumerator _runCoroutine;
    private Vector3 _lastTargetPosition;

    public override void Enter()
    {
        base.Enter();
        _runCoroutine = Run();
        StartCoroutine(_runCoroutine);
    }

    public override void Exit()
    {
        base.Exit();
        if (_runCoroutine != null)
            StopCoroutine(_runCoroutine);
        _tweener.Kill();
        _tweener = null;
    }

    private IEnumerator Run()
    {
        WaitForSeconds delay = new WaitForSeconds(0.02f);
        while (Finished == false)
        {
            yield return delay;
            if (_walkableArea.HasPointInArea(transform.position) == false)
            {
                OnExit?.Invoke();
                break;
            }
            if (_lastTargetPosition != _dangerousTransform.transform.position)
            {
                SetMovementAndRotation();
            }
        }
    }

    private void SetMovementAndRotation()
    {
        var posWithoutY = transform.position;
        var dangerousPosWithoutY = _dangerousTransform.transform.position;
        posWithoutY.y = 0;
        dangerousPosWithoutY.y = 0;
        var heading = posWithoutY - dangerousPosWithoutY;
        var distance = heading.magnitude;
        var direction = heading / distance;
        transform.forward = direction.normalized;

        _lastTargetPosition = _dangerousTransform.transform.position;

        if (_tweener == null)
        {
            _tweener = transform.DOMove(direction * distance + transform.position, GetTime()).SetAutoKill(false);
            _tweener.Play();
        }
        else
            _tweener.ChangeEndValue(direction * distance + transform.position, GetTime(), true).Restart();
    }

    private float GetDistance() =>
        Vector3.Distance(transform.position, _dangerousTransform.transform.position);

    private float GetTime() =>
        GetDistance() / _speed;
}

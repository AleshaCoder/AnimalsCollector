using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class FreeWalkingState : StateWithAnimator
{
    [SerializeField] private Transform _walkingTransform;
    [Tooltip("Units per second")]
    [SerializeField] private float _speed;

    private Tween _mainTween;
    private Animator _animator;
    private WalkableArea _walkableArea;
    private List<Vector3> _pathPoints = new List<Vector3>();

    public override void Enter()
    {
        base.Enter();
        _walkableArea = WalkableArea.Instance;
        CreateNewPathPoints();
        StartMovement();
        _animator.Play(AnimatorChick.States.Walk);
    }

    public override void Exit()
    {
        base.Exit();
        _mainTween.Pause();
        _mainTween.Kill();
    }

    private void CreateNewPathPoints()
    {
        var pointsCount = Random.Range(1, 10);
        _pathPoints.Clear();

        for (int i = 0; i < pointsCount; i++)
        {
            var newPoint = _walkableArea.GetRandomPointInArea();
            newPoint.y = _walkingTransform.position.y;
            _pathPoints.Add(newPoint);
        }
    }

    private void StartMovement()
    {
        var duration = GetPathCommonDistance() / _speed;
        _mainTween = _walkingTransform.DOPath(_pathPoints.ToArray(), duration, PathType.CatmullRom, PathMode.Full3D, 10, Color.red).SetOptions(true).SetLookAt(0.001f);
        _mainTween.SetEase(Ease.Linear);
        _mainTween.SetAutoKill(false);
        _mainTween.onComplete += () => OnExit?.Invoke();
        _mainTween.Play();
    }

    private float GetPathCommonDistance()
    {
        float distance = 0f;
        for (int i = 0; i < _pathPoints.Count - 1; i++)
        {
            distance += Vector3.Distance(_pathPoints[i], _pathPoints[i + 1]);
        }
        distance += Vector3.Distance(_pathPoints[0], _pathPoints[_pathPoints.Count - 1]);
        return distance;
    }

    private void Update()
    {
        if (_walkableArea == null)
            _walkableArea = WalkableArea.Instance;
        else
            _walkableArea.HasPointInArea(transform.position);
    }

    public override void Init(Animator animator) => _animator = animator;
}

using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class FreeWalkingState : StateWithFinish
{
    [SerializeField] private Transform _walkingTransform;
    [SerializeField] private WalkableArea _walkableArea;
    [Tooltip("Units per second")]
    [SerializeField] private float _speed;

    private Tween _mainTween;
    private List<Vector3> _pathPoints = new List<Vector3>();

    public override void Enter()
    {
        CreateNewPathPoints();
        StartMovement();
    }

    private void CreateNewPathPoints()
    {
        var pointsCount = Random.Range(1, 10);
        _pathPoints.Clear();

        for (int i = 0; i < pointsCount; i++)
            _pathPoints.Add(_walkableArea.GetRandomPointOnMesh());
    }

    private void StartMovement()
    {
        var duration = GetPathCommonDistance() / _speed;
        _mainTween = _walkingTransform.DOPath(_pathPoints.ToArray(), duration, PathType.CatmullRom, PathMode.Full3D, 10, Color.red).SetOptions(true).SetLookAt(0.001f);
        _mainTween.SetEase(Ease.Linear);
        _mainTween.SetAutoKill(false);
        _mainTween.onComplete += () => OnFinished?.Invoke();
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

    public override void Exit()
    {
        _mainTween.Kill();
    }
}

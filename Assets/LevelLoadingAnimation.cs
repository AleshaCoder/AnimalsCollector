using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;
using System;

public class LevelLoadingAnimation : MonoBehaviour
{
    [SerializeField] private LevelsPool _levelsPool;
    [SerializeField] private Transform _bridge;

    public Action OnLeaving;
    public Action OnStartArrival;
    public Action OnComplete;

    public async Task Play()
    {
        await PlayUnparking();
        await PlayLeaving();
        await Task.Delay(100);
        await PlayArrival();
        await PlayParking();
        OnComplete?.Invoke();
    }

    private async Task PlayUnparking()
    {
        var anim = _bridge.DOLocalRotate(new Vector3(0, 0, -20), 0.5f, RotateMode.Fast).Play();
        await anim.AsyncWaitForCompletion();
    }

    private async Task PlayLeaving()
    {
        var anim = _levelsPool.CurrentLevel.LevelObject.transform.DOLocalMove(new Vector3(-1, 0, -5), 3).Play();
        await anim.AsyncWaitForCompletion();
        OnLeaving?.Invoke();
    }

    private async Task PlayArrival()
    {
        OnStartArrival?.Invoke();
        _levelsPool.CurrentLevel.LevelObject.transform.localPosition = new Vector3(0, 0, 5);
        var anim = _levelsPool.CurrentLevel.LevelObject.transform.DOLocalMove(new Vector3(0, 0, 0), 2).Play();
        await anim.AsyncWaitForCompletion();
    }

    private async Task PlayParking()
    {
        var anim = _bridge.DOLocalRotate(new Vector3(0, 0, 0), 0.5f, RotateMode.Fast).Play();
        await anim.AsyncWaitForCompletion();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBehaviour : MonoBehaviour
{
    [SerializeField] private StateWithFinish _freeWalking;

    private IState _currentState;

    private void ChangeCurrentState()
    {
        _currentState?.Exit();
        _freeWalking.OnFinished = null;
        _freeWalking.OnFinished += ChangeCurrentState;
        _currentState = _freeWalking;
        _currentState.Enter();
    }

    private void Start()
    {
        ChangeCurrentState();
    }
}

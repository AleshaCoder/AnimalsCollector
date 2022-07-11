using ECM.Walkthrough.CustomInput;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour, IService
{
    [SerializeField] private bool _enabled;
    [SerializeField] private GameObject _view;
    [SerializeField] private GameObject _text;

    public Action OnTrigger;

    public void SwitchOn()
    {
        _enabled = true;
        _view.SetActive(_enabled);
        _text.SetActive(_enabled);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_enabled == false)
            return;
        if (other.TryGetComponent(out MyCharacterController movement))
        {
            OnTrigger?.Invoke();
            _enabled = false;
            _view.SetActive(_enabled);
            _text.SetActive(_enabled);
        }
    }
}

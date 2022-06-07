using System;
using System.Collections;
using UnityEngine;

public class HeroDetector : MonoBehaviour
{
    private const float MsInSec = 0.001f;
    [SerializeField] private Transform _hero;
    [SerializeField] private float _detectionDistance;
    [SerializeField] private float _lossDistance;
    [Tooltip("ms")][SerializeField] private float _updateTime;
    [SerializeField] private bool _active;

    [Header("Editor Settings")]
    [SerializeField] private float _distanceToHero;

    private bool _detected;

    public bool Detected => _detected;

    public Action OnDetection;
    public Action OnLoss;

    public void SwitchOn()
    {
        _active = true;
        StartCoroutine(CheckDetection());
    }

    public void SwitchOff()
    {
        OnDetection = null;
        OnLoss = null;
        _detected = false;
        _active = false;
    }

    private IEnumerator CheckDetection()
    {
        float _currentDistance;
        WaitForSeconds delay = new WaitForSeconds(_updateTime * MsInSec);

        while (_active)
        {
            _currentDistance = Vector3.Distance(_hero.position, transform.position);

            if (_detected == false && _detectionDistance >= _currentDistance)
            {
                _detected = true;
                OnDetection?.Invoke();
            }            
            if (_detected == true && _lossDistance <= _currentDistance)
            {
                _detected = false;
                OnLoss?.Invoke();
            }

            yield return delay;
        }
    }

    private void OnValidate()
    {
        _updateTime = _updateTime <= 0 ? -_updateTime + 1 : _updateTime;      

        if (_hero == null)
        {
            _hero = FindObjectOfType<Hero>().transform;
        }

        if (_hero.TryGetComponent(out Hero hero) == false)
        {
            _hero = null;
            Debug.LogError("Transform _hero should has type Hero");
        }

        if (_lossDistance < _detectionDistance)
        {
            Debug.LogWarning("LossDistance should be more than DetectionDistance");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        if (_hero != null)
        {
            Gizmos.DrawLine(_hero.position, transform.position);
            _distanceToHero = Vector3.Distance(_hero.position, transform.position);
        }
    }
}

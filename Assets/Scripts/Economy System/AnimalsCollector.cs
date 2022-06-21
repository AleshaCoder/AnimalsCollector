using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class AnimalsCollector : MonoBehaviour
{
    [SerializeField] private Transform _endPoint;
    [SerializeField] private float _collectingTime = 1f;
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private float _acceleration = 0.05f;
    [SerializeField] private float _minCollectingTime = 0.3f;
    public Action<Animal> OnCollect;

    private IEnumerator _collectCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CollectedAnimals animals))
        {
            if (_collectCoroutine != null)
                StopCoroutine(_collectCoroutine);
            _collectCoroutine = Collect(animals);
            StartCoroutine(_collectCoroutine);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out CollectedAnimals animals))
        {
            if (_collectCoroutine != null)
                StopCoroutine(_collectCoroutine);
        }
    }

    private IEnumerator Collect(CollectedAnimals animals)
    {
        float duration = _collectingTime;
        var delay = new WaitForSeconds(duration);
        while (animals.HasAnimals())
        {
            Animal animal = animals.TakeAnimal();
            OnCollect?.Invoke(animal);
            animal.transform.parent = _endPoint;
            var animMove = animal.transform.DOLocalJump(Vector3.zero, _jumpHeight, 1, duration);
            animMove.Play();
            yield return delay;
            duration -= _acceleration;
            if (duration < _minCollectingTime)
                duration = _minCollectingTime;
            delay = new WaitForSeconds(duration);
        }
    }
}

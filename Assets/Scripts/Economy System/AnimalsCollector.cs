using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class AnimalsCollector : MonoBehaviour
{
    [SerializeField] private Transform _endPoint;
    public Action<Animal> OnCollect;

    private IEnumerator _collectCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CollectedAnimals animals))
        {
            if (_collectCoroutine != null)
                StopCoroutine(_collectCoroutine);
            _collectCoroutine = Collect(animals);
            Debug.Log("Start Collecting");
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
        float duration = 1f;
        var delay = new WaitForSeconds(duration);
        Debug.Log($" animals have {animals.HasAnimals()}");
        while (animals.HasAnimals())
        {
            Debug.Log("Collect");
            Animal animal = animals.TakeAnimal();
            OnCollect?.Invoke(animal);
            animal.transform.parent = _endPoint;
            var animMove = animal.transform.DOLocalJump(Vector3.zero, 5, 1, duration);
            animMove.Play();
            yield return delay;
            //Destroy(animal.gameObject);
        }
    }
}

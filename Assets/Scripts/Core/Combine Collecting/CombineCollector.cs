using DG.Tweening;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class CombineCollector : MonoBehaviour, IService
{
    [SerializeField] private Transform _collectionPoint; // EndPoint Transform for pickable subjects
    [SerializeField] private Transform _enterCombinePoint;
    [SerializeField] private Transform _exitCombinePoint;
    [SerializeField] private PlacesGroup _placesGroup;
    [SerializeField] private GameObject _cellPrefab;

    public Action<IPickable> OnPick;
    public Action OnCollect;

    private async void Pick(IPickable pickable)
    {
        if (_placesGroup.TryTakeFreePlace(out Place animalPlace))
        {
            OnCollect?.Invoke();
            Transform pickableTransform = ((MonoBehaviour)pickable).transform;
            pickableTransform.position = _collectionPoint.position;
            pickableTransform.parent = transform;
            pickable.Pick();
            await MoveInto(pickableTransform);
            await MoveToExit(pickableTransform);
            await ThrowIntoPlace(pickableTransform, animalPlace);
            OnPick?.Invoke(pickable);
        }
    }

    private async Task MoveInto(Transform pickableTransform)
    {
        float duration = 0.3f;
        var anim = pickableTransform.DOLocalMove(_enterCombinePoint.localPosition, duration);
        anim.Play();
        await anim.AsyncWaitForCompletion();
    }

    private async Task MoveToExit(Transform pickableTransform)
    {
        float duration = 0.5f;
        var anim = pickableTransform.DOLocalMove(_exitCombinePoint.localPosition, duration);
        anim.Play();
        await anim.AsyncWaitForCompletion();
    }

    private async Task ThrowIntoPlace(Transform pickableTransform, Place place)
    {
        float duration = 0.7f;

        var prefab = Instantiate(_cellPrefab, pickableTransform);
        prefab.transform.localScale = new Vector3(110, 110, 6);
        prefab.transform.localPosition = new Vector3(0, 0.2f, 0);
        prefab.transform.localEulerAngles = new Vector3(90, 0, 0);
        pickableTransform.parent = place.transform;
        pickableTransform.localEulerAngles = Vector3.zero;
        pickableTransform.localScale = new Vector3(0.04f, 0.2f, 0.04f);
        //pickableTransform.localPosition = Vector3.zero;
        var anim = pickableTransform.DOLocalJump(Vector3.zero, 2, 1, duration);
        anim.Play();
        await anim.AsyncWaitForCompletion();
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IPickable pickable))
        {
            Pick(pickable);
        }
    }
}

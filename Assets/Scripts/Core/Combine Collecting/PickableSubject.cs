using System;
using System.Collections;
using UnityEngine;

public class PickableSubject : MonoBehaviour, IPickable
{
    [SerializeField]
    [Header("Time for pick")]
    private float _pickableSpeed;
    private bool _readyToPick = true;

    public bool ReadyToPick { get => _readyToPick; set => _readyToPick = value; }

    public event Action OnPick;

    public void Pick()
    {
        if (!_readyToPick) return;
        OnPick?.Invoke();
        //StartCoroutine(DoStack(targetTransform));
    }

    private IEnumerator DoStack(Transform targetTransform)
    {
        transform.parent = targetTransform;
        while (transform.position != targetTransform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, _pickableSpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PickableSubject : MonoBehaviour, IPickable
{
    [SerializeField]
    [Header("Time for pick")]
    private float _pickableSpeed;
    private bool _readyToPick = true;
    public bool ReadyToPick { get => _readyToPick; set => _readyToPick = value; }

    public void Pick(Transform targetTransform)
    {
        if (!_readyToPick) return;

         StartCoroutine(DoStack(targetTransform));
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

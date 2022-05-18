using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineCollector : MonoBehaviour
{
    [SerializeField]
    private Transform _collectorPoint; // EndPoint Transform for pickable subjects

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IPickable pickable))
        {
            pickable.Pick(_collectorPoint);
        }
    }
}

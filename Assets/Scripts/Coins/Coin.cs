using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    public void MoveTo(Transform target)
    {
        var anim = transform.DOMove(target.position, 1f).Play();
        var anim2 = transform.DOScale(1.6f, 1f).Play();
        anim.onComplete += Destroy;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}

using System;
using UnityEngine;

public class Animal : MonoBehaviour, ISellable, IPickable
{
    [SerializeField] private int _price;
    public int Price { get => _price; private set => _price = Mathf.Abs(value); }

    public event Action OnPick;
    public void Pick()
    {
        OnPick?.Invoke();
    }
}

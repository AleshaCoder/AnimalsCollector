using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Economy : IService
{
    private Money _gold;

    public int GoldCount => _gold.Count;

    public event Action<int> OnGoldChanged;

    public Economy()
    {
        _gold = new Money(1000);
    }

    public void AddGold(int count)
    {
        _gold.Add(count);
        OnGoldChanged?.Invoke(_gold.Count);
    }

    public bool TrySpendGold(int count)
    {
        var can = _gold.TryGetMoney(count);
        OnGoldChanged?.Invoke(_gold.Count);
        return can;
    }
}

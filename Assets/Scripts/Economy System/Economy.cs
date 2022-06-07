using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Economy : IService
{
    private Money _gold;

    public Economy()
    {
        _gold = new Money(10000);
    }

    public void AddGold(int count)
    {
        _gold.Add(count);
        Debug.Log($"Money {_gold.Count}");
    }

    public bool TrySpendGold(int count)
    {
        var can = _gold.TryGetMoney(count);
        Debug.Log($"Money {_gold.Count}");
        return can;
    }
}

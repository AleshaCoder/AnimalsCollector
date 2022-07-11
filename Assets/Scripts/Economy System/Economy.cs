using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Economy : IService
{
    private const string Gold = "gold";
    private Money _gold;

    public int GoldCount => _gold.Count;

    public event Action<int> OnGoldChanged;

    public Economy()
    {
        int count = PlayerPrefs.GetInt(Gold, 500);
        _gold = new Money(count);
        OnGoldChanged += (count) => Save();
    }

    ~Economy()
    {
        OnGoldChanged = null;
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

    private void Save()
    {
        PlayerPrefs.SetInt(Gold, _gold.Count);
        PlayerPrefs.Save();
    }
}

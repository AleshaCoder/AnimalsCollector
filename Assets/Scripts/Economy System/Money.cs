using System;

public class Money
{
    private int _count;

    public int Count => _count;

    public Action<int> OnCountChanged;

    public Money(int count)
    {
        _count = count;
        OnCountChanged?.Invoke(Count);
    }

    public void Add(int count)
    {
        _count += count;
        OnCountChanged?.Invoke(Count);
    }

    public bool TryGetMoney(int count)
    {
        bool enoughMoney = _count >= count;

        if (enoughMoney)
        {
            _count -= count;
            OnCountChanged?.Invoke(Count);
        }
        return enoughMoney;
    }

}

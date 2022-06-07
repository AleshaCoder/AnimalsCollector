using System;
using UnityEngine;

public interface IPickable
{
    event Action OnPick;
    void Pick();
}

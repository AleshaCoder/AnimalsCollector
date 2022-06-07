using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgradable
{
    public int Level { get; }
    public void Upgrade();
}

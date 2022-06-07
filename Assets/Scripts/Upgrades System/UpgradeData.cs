using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class UpgradeData
{
    [System.Serializable]
    public struct Upgrade
    {
        public int Level;
        public int Result;
    }

    [SerializeField] private List<Upgrade> _upgrades;

    public int GetResult(int level)
        => _upgrades.Where(upgrade => upgrade.Level == level).First().Result;
}

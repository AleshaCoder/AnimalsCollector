using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradableCollector : MonoBehaviour
{
    [System.Serializable]
    public struct Upgrade
    {
        public int Level;
        public Vector3 ColliderSize;
    }
    [SerializeField] private BoxCollider _collider;

    [SerializeField] private List<Upgrade> _upgrades;

    public void SetUpgrade(int level)
    {
        _collider.size = _upgrades.Where(upgrade => upgrade.Level == level).First().ColliderSize;
    }

    private void Awake()
    {
        _collider.size = _upgrades[0].ColliderSize;
    }
}

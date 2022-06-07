using System.Collections.Generic;
using UnityEngine;

public class UpgradesValidator : MonoBehaviour
{
    [SerializeField] private SpeedUpgrade _speedUpgrade;
    [SerializeField] private CapacityUpgrade _capacityUpgrade;
    [SerializeField] private GrabUpgrade _grabUpgrade;
    [Space]
    [SerializeField] private UpgradeView _speedUpgradeView;
    [SerializeField] private UpgradeView _capacityUpgradeView;
    [SerializeField] private UpgradeView _grabUpgradeView;

    [SerializeField] private List<int> _upgradesPrices;

    private Economy _economy;

    public void UpgradeSpeed()
    {
        if (_economy.TrySpendGold(_upgradesPrices[_speedUpgrade.Level]))
            _speedUpgrade.Upgrade();
        _speedUpgradeView.RefreshLevelAsResult(_speedUpgrade.GetResult(_speedUpgrade.Level), _speedUpgrade.GetResult(_speedUpgrade.Level + 2));
        _speedUpgradeView.RefreshPrice(_upgradesPrices[_speedUpgrade.Level]);
    }

    public void UpgradeCapacity()
    {
        if (_economy.TrySpendGold(_upgradesPrices[_capacityUpgrade.Level]))
            _capacityUpgrade.Upgrade();
        _capacityUpgradeView.RefreshLevelAsResult(_capacityUpgrade.GetResult(_capacityUpgrade.Level), _capacityUpgrade.GetResult(_capacityUpgrade.Level + 2));
        _capacityUpgradeView.RefreshPrice(_upgradesPrices[_capacityUpgrade.Level]);
    }

    public void UpgradeGrab()
    {
        if (_economy.TrySpendGold(_upgradesPrices[_grabUpgrade.Level]))
            _grabUpgrade.Upgrade();
        _grabUpgradeView.RefreshLevelAsLevel(_grabUpgrade.Level);
        _grabUpgradeView.RefreshPrice(_upgradesPrices[_grabUpgrade.Level - 1]);
    }

    private void Start()
    {
        _economy = Services.Container.Single<Economy>();
        _speedUpgradeView.OnClick += UpgradeSpeed;
        _capacityUpgradeView.OnClick += UpgradeCapacity;
        _grabUpgradeView.OnClick += UpgradeGrab;
        UpgradeSpeed();
        UpgradeCapacity();
        UpgradeGrab();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UpgradeSpeed();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            UpgradeCapacity();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            UpgradeGrab();
        }
    }
}

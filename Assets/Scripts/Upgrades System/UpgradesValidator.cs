using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UpgradesValidator : MonoBehaviour, IService
{
    private class UpgradeData
    {
        public IUpgradable Upgrade;
        public UpgradeView UpgradeView;

        public UpgradeData(IUpgradable upgrade, UpgradeView upgradeView)
        {
            Upgrade = upgrade;
            UpgradeView = upgradeView;
        }
    }

    [SerializeField] private SpeedUpgrade _speedUpgrade;
    [SerializeField] private CapacityUpgrade _capacityUpgrade;
    [SerializeField] private GrabUpgrade _grabUpgrade;
    [Space]
    [SerializeField] private UpgradeView _speedUpgradeView;
    [SerializeField] private UpgradeView _capacityUpgradeView;
    [SerializeField] private UpgradeView _grabUpgradeView;

    [SerializeField] private List<int> _upgradesPrices;

    private Economy _economy;
    private List<UpgradeData> _upgradeDatas;

    private void InitAll()
    {
        foreach (var item in _upgradeDatas)
        {
            item.Upgrade.Init();
            item.UpgradeView.OnClick = () => Upgrade(item);
            item.UpgradeView.RefreshLevelAsLevel(item.Upgrade.Level);
            item.UpgradeView.RefreshPrice(_upgradesPrices[item.Upgrade.Level]);
        }
    }

    private void Upgrade(UpgradeData data)
    {
        if (_economy.TrySpendGold(_upgradesPrices[data.Upgrade.Level]))
        {
            data.Upgrade.Upgrade();
            data.UpgradeView.RefreshLevelAsLevel(data.Upgrade.Level);
            data.UpgradeView.RefreshPrice(_upgradesPrices[data.Upgrade.Level]);
        }
    }

    public void Init()
    {
        _economy = Services.Container.Single<Economy>();
        _upgradeDatas = new List<UpgradeData>();
        _upgradeDatas.Add(new UpgradeData(_speedUpgrade, _speedUpgradeView));
        _upgradeDatas.Add(new UpgradeData(_capacityUpgrade, _capacityUpgradeView));
        _upgradeDatas.Add(new UpgradeData(_grabUpgrade, _grabUpgradeView));
        InitAll();
    }
}

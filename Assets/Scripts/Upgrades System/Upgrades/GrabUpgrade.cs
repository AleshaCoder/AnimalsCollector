using UnityEngine;

[System.Serializable]
public class GrabUpgrade : IUpgradable
{
    [SerializeField] private int _level;
    [SerializeField] private UpgradableCollector _upgradableCollector;
    public int Level => _level;

    public void Upgrade()
    {
        _level++;
        _upgradableCollector.SetUpgrade(Level);
        Debug.Log("GrabUpgrade");
    }
}

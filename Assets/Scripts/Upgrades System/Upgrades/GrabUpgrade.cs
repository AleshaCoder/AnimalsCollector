using UnityEngine;

[System.Serializable]
public class GrabUpgrade : IUpgradable
{
    private const string LevelGrab = "LevelGrab";
    [SerializeField] private int _level;
    [SerializeField] private UpgradableCollector _upgradableCollector;
    public int Level => _level;

    public void Init()
    {
        int level = PlayerPrefs.GetInt(LevelGrab, 1);
        _level = level;
        _upgradableCollector.SetUpgrade(_level);
    }

    public void Upgrade()
    {
        _level++;
        _upgradableCollector.SetUpgrade(Level);
        PlayerPrefs.SetInt(LevelGrab, _level);
        PlayerPrefs.Save();
    }
}

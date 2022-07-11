using UnityEngine;
using ECM.Walkthrough.CustomInput;

[System.Serializable]
public class SpeedUpgrade : IUpgradable
{
    private const string LevelSpeed = "LevelSpeed";
    [SerializeField] private int _level;
    [SerializeField] private MyCharacterController _characterController;
    [SerializeField] private UpgradeData _upgradeData;
    public int Level => _level;

    public void Init()
    {
        int level = PlayerPrefs.GetInt(LevelSpeed, 1);
        _level = level;
    }

    public int GetResult(int level) => _upgradeData.GetResult(level);

    public void Upgrade()
    {
        _level++;
        _characterController.speed = _upgradeData.GetResult(_level);
        PlayerPrefs.SetInt(LevelSpeed, _level);
        PlayerPrefs.Save();
    }
}

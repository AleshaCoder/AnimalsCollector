using UnityEngine;
using ECM.Walkthrough.CustomInput;

[System.Serializable]
public class SpeedUpgrade : IUpgradable
{
    [SerializeField] private int _level;
    [SerializeField] private MyCharacterController _characterController;
    [SerializeField] private UpgradeData _upgradeData;
    public int Level => _level;

    public int GetResult(int level)
    {
        return _upgradeData.GetResult(level); 
    }

    public void Upgrade()
    {
        _level++;
        _characterController.speed = _upgradeData.GetResult(_level);
    }
}

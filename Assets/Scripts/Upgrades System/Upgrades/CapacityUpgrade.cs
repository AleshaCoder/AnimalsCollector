using UnityEngine;
using DG.Tweening;
using ECM.Walkthrough.CustomInput;

[System.Serializable]
public class CapacityUpgrade : IUpgradable
{
    private const string LevleCapacity = "LevelCapacity";
    [SerializeField] private int _level;
    [SerializeField] private PlacesGroup _placesGroup;
    [SerializeField] private UpgradeData _upgradeData;

    public int Level => _level;

    public int GetResult(int level)
    {
        return _upgradeData.GetResult(level);
    }

    public void Init()
    {
        int level = PlayerPrefs.GetInt(LevleCapacity, 1);
        _level = level;
        _placesGroup.FreePlaceCount = _upgradeData.GetResult(_level);
    }

    public void Upgrade()
    {
        _level++;
        _placesGroup.FreePlaceCount = _upgradeData.GetResult(_level);
        PlayerPrefs.SetInt(LevleCapacity, _level);
        PlayerPrefs.Save();

        var _characterController = Object.FindObjectOfType<MyCharacterController>();
        float scale = _characterController.transform.localScale.x;
        _characterController.transform.localScale = _characterController.transform.localScale * 0.8f;
        var anim = _characterController.transform.DOScale(scale * 1.2f, 0.3f).Play();
        anim.onComplete += () => _characterController.transform.DOScale(scale, 0.3f).Play();
    }
}

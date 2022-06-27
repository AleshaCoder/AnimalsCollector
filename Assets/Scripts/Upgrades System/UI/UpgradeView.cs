using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UpgradeView : MonoBehaviour
{
    [SerializeField] private TMP_Text _price, _level;
    [SerializeField] private LevelsIconsGroup _levelsIconsGroup;
    [SerializeField] private Button _upButton;

    public Action OnClick;

    public void RefreshPrice(int price) => _price.text = price.ToString();

    public void RefreshLevelAsLevel(int level)
    {
        _levelsIconsGroup.SetLevel(level);
        if (_level != null)
            _level.text = level.ToString();
    }

    private void OnEnable() => _upButton.onClick.AddListener(() => OnClick?.Invoke());

    private void OnDisable() => _upButton.onClick.RemoveAllListeners();
}

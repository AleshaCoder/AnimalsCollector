using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LevelsIconsGroup : MonoBehaviour
{
    [SerializeField] private Sprite _activeRectPrefab;
    [SerializeField] private Sprite _activeRightPrefab;

    [SerializeField] private List<Sprite> _activeRectPrefabs;

    [SerializeField] private Image _leftIcon;
    [SerializeField] private Image _rightIcons;
    [SerializeField] private List<Image> _rectIcons;

    [SerializeField] private int _maxLevel = 10;
    private int _level;


    public void SetLevel(int level)
    {
        for (int i = 0; i < Mathf.Min(_maxLevel - 2, level-1); i++)
            _rectIcons[i].sprite = _activeRectPrefabs[i];

        if (level == _maxLevel-1)
            _rightIcons.sprite = _activeRectPrefabs.Last();
    }
}

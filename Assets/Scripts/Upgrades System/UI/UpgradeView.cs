using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UpgradeView : MonoBehaviour
{
    [SerializeField] private TMP_Text _price, _level;
    [SerializeField] private Button _upButton;

    public Action OnClick;

    public void RefreshPrice(int price)
    {
        _price.text = price.ToString();
    }

    public void RefreshLevelAsLevel(int level)
    {
        _level.text = level.ToString();
    }

    public void RefreshLevelAsResult(int result, int nextResult)
    {
        _level.text = $"{result} -> {nextResult}";
    }

    private void OnEnable()
    {
        _upButton.onClick.AddListener(() => OnClick?.Invoke());
    }

    private void OnDisable()
    {
        _upButton.onClick.RemoveListener(() => OnClick?.Invoke());
    }
}

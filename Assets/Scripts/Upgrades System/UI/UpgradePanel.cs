using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private Button _exit;
    public Action OnExit;

    private void OnEnable() => _exit.onClick.AddListener(() => OnExit?.Invoke());

    private void OnDisable() => _exit.onClick.RemoveAllListeners();
}

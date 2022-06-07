using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject _lose, _win, _main;
    [SerializeField] private Button _loseButton, _winButton;
    private static ScreenManager _screenManager;

    public static Action OnWin, OnLose;

    public static void ShowLose()
    {
        _screenManager._lose.SetActive(true);
        _screenManager._win.SetActive(false);
        _screenManager._main.SetActive(false);
    }

    public static void ShowWin()
    {
        _screenManager._lose.SetActive(false);
        _screenManager._win.SetActive(true);
        _screenManager._main.SetActive(false);
    }

    public static void ShowMain()
    {
        _screenManager._lose.SetActive(false);
        _screenManager._win.SetActive(false);
        _screenManager._main.SetActive(true);
    }

    private void Awake()
    {
        _loseButton.onClick.AddListener(() => OnLose?.Invoke());
        _winButton.onClick.AddListener(() => OnWin?.Invoke());
        _screenManager = this;
    }
}

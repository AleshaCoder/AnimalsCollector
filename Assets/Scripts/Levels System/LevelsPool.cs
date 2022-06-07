using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsPool : MonoBehaviour
{
    [SerializeField] private List<Level> _levels;
    private Level _currentLevel;
    private int _index = 0;

    public IReadOnlyCollection<Level> GetLevels() => _levels;

    public Level CurrentLevel => _currentLevel;

    public Level GetNextLevel()
    {
        _index++;
        Debug.Log(_index + " Level");
        _currentLevel = _levels[_index];
        return _currentLevel;
    }

    public void Init()
    {
        if (PlayerPrefs.HasKey("level"))
        {
            _currentLevel = _levels[PlayerPrefs.GetInt("level")];
            _index = PlayerPrefs.GetInt("level");
        }
        else
        {
            _currentLevel = _levels[0];
        }
    }

    private void OnApplicationPause(bool pause)
    {
        PlayerPrefs.SetInt("level", _index);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("level", _index);
        PlayerPrefs.Save();
    }
}

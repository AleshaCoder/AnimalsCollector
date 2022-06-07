using UnityEngine;
using UnityEngine.Events;

public class LevelLoader : MonoBehaviour, IService
{
    [SerializeField] private LevelsPool _levelsPool;

    public UnityAction<Level> OnLevelChanged;

    public void LoadCurrentLevel()
    {
        _levelsPool.CurrentLevel.Load();
        OnLevelChanged?.Invoke(_levelsPool.CurrentLevel);
    }

    public void LoadNextLevel()
    {
        _levelsPool.CurrentLevel.Unload();
        _levelsPool.GetNextLevel().Load();
        OnLevelChanged?.Invoke(_levelsPool.CurrentLevel);
    }

    public void ReloadLevel()
    {
        _levelsPool.CurrentLevel.Unload();
        _levelsPool.CurrentLevel.Load();
        OnLevelChanged?.Invoke(_levelsPool.CurrentLevel);
    }
}

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class LevelLoader : MonoBehaviour, IService
{
    [SerializeField] private LevelsPool _levelsPool;
    [SerializeField] private Transform _parentForLevel;
    [SerializeField] private LevelLoadingAnimation _levelLoadingAnimation;

    public UnityAction OnStartLevelChanged;
    public UnityAction<Level> OnLevelChanged;

    public void LoadCurrentLevel()
    {
        OnStartLevelChanged?.Invoke();
        _levelsPool.CurrentLevel.Load(_parentForLevel, Vector3.zero);
        OnLevelChanged?.Invoke(_levelsPool.CurrentLevel);
    }

    public async Task LoadNextLevel()
    {
        OnStartLevelChanged?.Invoke();
        _levelLoadingAnimation.OnLeaving += Unload;
        _levelLoadingAnimation.OnStartArrival += Load;
        //_levelLoadingAnimation.OnStartArrival += Debug.Break;
        await _levelLoadingAnimation.Play();
        _levelLoadingAnimation.OnLeaving = null;
        _levelLoadingAnimation.OnStartArrival = null;
        OnLevelChanged?.Invoke(_levelsPool.CurrentLevel);
    }

    public void ReloadLevel()
    {
        Unload();
        _levelsPool.CurrentLevel.Load(_parentForLevel, Vector3.zero);
        OnLevelChanged?.Invoke(_levelsPool.CurrentLevel);
    }

    private void Load()
    {
        _levelsPool.GetNextLevel().Load(_parentForLevel, new Vector3(0, 0, 5));
    }

    private void Unload()
    {
        _levelsPool.CurrentLevel.Unload();
    }
}

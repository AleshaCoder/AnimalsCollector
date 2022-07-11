using UnityEngine;

public class ServicesInstance : MonoBehaviour, IService
{
    [Header("Needs to implement " + nameof(IService))]
    [SerializeField] private CameraFollower _cameraFollower;
    [SerializeField] private LevelLoader _levelLoader;
    [SerializeField] private UpgradesValidator _upgradeValidator;
    [SerializeField] private LevelTrigger _levelTrigger;
    [SerializeField] private LevelsPool _levelsPool;
    [SerializeField] private CombineCollector _combineCollector;
    [SerializeField] private Hero _hero;


    public void Init()
    {
        Services.Container.RegisterSingle(_cameraFollower);
        Services.Container.RegisterSingle(_levelLoader);
        Services.Container.RegisterSingle(_upgradeValidator);
        Services.Container.RegisterSingle(_levelTrigger);
        Services.Container.RegisterSingle(_levelsPool);
        Services.Container.RegisterSingle(_combineCollector);
        Services.Container.RegisterSingle(_hero);
    }

}

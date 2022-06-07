using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class ServicesInstance : MonoBehaviour, IService
{
    [Header("Needs to implement " + nameof(IService))]
    [SerializeField] private CameraFollower _cameraFollower;
    [SerializeField] private GameStarter _gameStarter;
    [SerializeField] private LevelLoader _levelLoader;


    public void Awake()
    {
        Services.Container.RegisterSingle<CameraFollower>(_cameraFollower);
        Services.Container.RegisterSingle<GameStarter>(_gameStarter);
        Services.Container.RegisterSingle<LevelLoader>(_levelLoader);
    }

}

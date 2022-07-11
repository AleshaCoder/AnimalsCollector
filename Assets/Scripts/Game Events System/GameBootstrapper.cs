using UnityEngine;

public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
{
    public LoadingCurtain Curtain;
    public ServicesInstance ServicesInstance;
    private Game _game;

    private void Awake()
    {
        _game = new Game(this, Curtain);
        ServicesInstance.Init();
        _game.StateMachine.Enter<BootstrapState>();
        DontDestroyOnLoad(this);
    }
}

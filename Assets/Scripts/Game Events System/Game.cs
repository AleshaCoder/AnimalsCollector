using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public GameStateMachine StateMachine;

    public Game(ICoroutineRunner coroutineRunner, LoadingCurtain curtain)
    {
        StateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), curtain);
    }
}

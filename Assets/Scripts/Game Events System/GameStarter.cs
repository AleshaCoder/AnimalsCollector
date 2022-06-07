using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GameStarter : MonoBehaviour, IPointerDownHandler, IService
{
    public static UnityAction OnGameStarted;
    public static UnityAction OnGameStoped;
    public static int CountGames;
    private bool _startAlready = false;
    private bool _stopAlready = false;

    private void Awake()
    {
        CountGames = PlayerPrefs.GetInt("gamesCount", 0);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_startAlready == true)
            return;
        _startAlready = true;
        _stopAlready = false;
        CountGames++;
        PlayerPrefs.SetInt("gamesCount", CountGames);
        OnGameStarted?.Invoke();
    }

    public void Stop()
    {
        if (_stopAlready == true)
            return;
        _startAlready = false;
        _stopAlready = true;
        OnGameStoped?.Invoke();
    }
}

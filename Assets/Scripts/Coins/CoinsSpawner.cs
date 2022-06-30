using UnityEngine;

public class CoinsSpawner : MonoBehaviour
{
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _parent;

    public void Spawn()
    {
        var screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Coin coin = Instantiate(_coinPrefab, _parent);
        coin.transform.position = screenPoint;
        coin.MoveTo(_target);
    }
}

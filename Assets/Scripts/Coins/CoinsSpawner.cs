using UnityEngine;

public class CoinsSpawner : MonoBehaviour
{
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private Transform _target;

    public void Spawn()
    {
        Coin coin = Instantiate(_coinPrefab, transform);
        coin.transform.localPosition = Vector3.zero;
        coin.transform.LookAt(Camera.main.transform);
        coin.MoveTo(_target);
    }
}

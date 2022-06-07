using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private GameObject _loadedObject;
    [SerializeField] private int _id;

    private GameObject _inst;
    public int ID => _id;

    public void Load()
    {
        _inst = Instantiate(_loadedObject);
        _inst.SetActive(true);
    }

    public void Unload()
    {
        Destroy(_inst);
    }
}

using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private GameObject _loadedObject;
    [SerializeField] private int _id;
    [SerializeField] private int _targetForNext;

    private int _copyTarget;
    private GameObject _inst;

    public int ID => _id;
    public int TargetForNext => _targetForNext;
    public GameObject LevelObject => _inst;

    public Action OnTarget;

    public void Decrease()
    {
        _targetForNext--;
        if (_targetForNext == 0)
        {
            Debug.Log("Level end");
            OnTarget?.Invoke();
            _targetForNext = _copyTarget;
        }
    }

    public void Load(Transform parent, Vector3 localPosition)
    {
        Debug.Log("Level "+name);
        _copyTarget = _targetForNext;
        _inst = Instantiate(_loadedObject, parent);
        _inst.transform.localPosition = localPosition;
        _inst.SetActive(true);
    }

    public void Load()
    {
        _copyTarget = _targetForNext;
        _inst = Instantiate(_loadedObject);
        _inst.SetActive(true);
    }

    public void Unload()
    {
        Destroy(_inst);
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacesGroup : MonoBehaviour
{
    public enum GroupType
    {
        FlatXY, FlatXZ, FlatYZ, Volume
    }

    [SerializeField] private GroupType _groupType;
    [SerializeField] private Vector3 _startVector;

    [SerializeField] private int _placesCount;

    [SerializeField] private float _xDistanceBetweenPlaces;
    [SerializeField] private float _yDistanceBetweenPlaces;
    [SerializeField] private float _zDistanceBetweenPlaces;

    [SerializeField] private int _maxCountX;
    [SerializeField] private int _maxCountY;
    [SerializeField] private int _maxCountZ;

    [SerializeField] private Place _prefab;
    [SerializeField] private List<Place> _places;
    [SerializeField] private int _freePlaceCount;

    [Header("Editor")]
    [SerializeField] private int _maxCount;
    [SerializeField] private bool _debugCubes = true;

    public int TakenPlacesCount => GetTakenPlacesCount();

    public int FreePlaceCount
    {
        get => _freePlaceCount;
        set
        {
            if (value < 0)
                return;

            if (value > _places.Count)
                _freePlaceCount = _places.Count;
            else
                _freePlaceCount = value;
            OnFreePlacesCountChanged?.Invoke();
        }
    }

    public event Action OnFreePlacesCountChanged;

    public bool TryTakeFreePlace(out Place animalPlace)
    {
        if (HasFreePlaces() == false)
        {
            animalPlace = null;
            return false;
        }

        foreach (var place in _places)
        {
            if (place.IsFree)
            {
                animalPlace = place;
                animalPlace.Take();
                OnFreePlacesCountChanged?.Invoke();
                return true;
            }
        }
        animalPlace = null;
        return false;
    }

    public void FreeAllPlaces()
    {
        foreach (var place in _places)
        {
            if (place.IsFree)
                continue;
            FreePlace(place);
        }
        OnFreePlacesCountChanged?.Invoke();
    }

    public void FreePlace()
    {
        for (int i = 0; i < _places.Count; i++)
        {
            if (_places[_places.Count - 1 - i].IsFree == false)
            {
                FreePlace(_places[_places.Count - 1 - i]);
                break;
            }
        }
    }

    public void FreePlace(Place place)
    {
        place.Free();
        OnFreePlacesCountChanged?.Invoke();
    }

    private bool HasFreePlaces()
    {
        if (GetTakenPlacesCount() < FreePlaceCount)
            return true;

        return false;
    }

    public int GetTakenPlacesCount()
    {
        int count = 0;
        foreach (var item in _places)
        {
            if (item.IsFree == false)
                count++;
        }
        return count;
    }

    [ContextMenu("Regenerate")]
    private void Regenerate()
    {
        if (_prefab == null)
            return;
        for (int i = 0; i < _places.Count; i++)
            DestroyImmediate(_places[i].gameObject, true);

        _places = new List<Place>();
        var last = _placesCount;

        for (int i = 0; i < _maxCountX; i++)
        {
            for (int j = 0; j < _maxCountY; j++)
            {
                for (int k = 0; k < _maxCountZ; k++)
                {
                    if (last == 0)
                        return;
                    last--;
                    Place animalPlace = Instantiate(_prefab, transform);
                    Vector3 newPosition;

                    switch (_groupType)
                    {
                        case GroupType.FlatXY:
                            newPosition = _startVector + new Vector3(i * _xDistanceBetweenPlaces, j * _yDistanceBetweenPlaces, 0);
                            break;
                        case GroupType.FlatXZ:
                            newPosition = _startVector + new Vector3(i * _xDistanceBetweenPlaces, 0, k * _zDistanceBetweenPlaces);
                            break;
                        case GroupType.FlatYZ:
                            newPosition = _startVector + new Vector3(0, j * _yDistanceBetweenPlaces, k * _zDistanceBetweenPlaces);
                            break;
                        case GroupType.Volume:
                            newPosition = _startVector + new Vector3(i * _xDistanceBetweenPlaces, j * _yDistanceBetweenPlaces, k * _zDistanceBetweenPlaces);
                            break;
                        default:
                            newPosition = Vector3.zero;
                            break;
                    }
                    animalPlace.transform.position = newPosition;
                    _places.Add(animalPlace);
                }
            }
        }
    }

    private void OnValidate()
    {
        foreach (var item in _places)
        {
            item.DrawCube = _debugCubes;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedAnimals : MonoBehaviour
{
    [SerializeField] private CombineCollector _combineCollector;
    [SerializeField] private PlacesGroup _placesGroup;
    private List<Animal> _animals;

    private void TryAdd(IPickable obj)
    {
        if (obj is Animal)
            Add(obj as Animal);
    }

    private void Add(Animal animal)
    {
        if (!_animals.Contains(animal))
            _animals.Add(animal);
    }

    public bool HasAnimals()
        => _animals.Count > 0;

    public Animal TakeAnimal()
    {
        _placesGroup.FreePlace();

        Animal animal = _animals[_animals.Count - 1];
        _animals.Remove(animal);

        return animal;
    }

    public List<Animal> TakeAllAnimals()
    {
        _placesGroup.FreeAllPlaces();
        List<Animal> animals = new List<Animal>();
        foreach (var item in _animals)
        {
            animals.Add(item);
        }
        _animals.Clear();
        return animals;
    }

    private void OnEnable()
    {
        _animals = new List<Animal>();
        _combineCollector.OnPick += TryAdd;
    }

    private void OnDisable()
    {
        _combineCollector.OnPick -= TryAdd;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalsToMoneyConverter : MonoBehaviour
{
    [SerializeField] private AnimalsCollector _animalCollector;
    [SerializeField] private CoinsSpawner _coinsSpawner;
    private Economy _economy;

    private void OnEnable()
    {
        _animalCollector.OnCollect += Convert;        
    }

    private void OnDisable()
    {
        _animalCollector.OnCollect -= Convert;
    }

    private void Start()
    {
        _economy = Services.Container.Single<Economy>();
    }

    private void Convert(Animal animal)
    {
        _economy.AddGold(animal.Price);
        _coinsSpawner.Spawn();
    }
}

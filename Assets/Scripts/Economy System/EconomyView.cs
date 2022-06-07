using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyView : MonoBehaviour
{
    private Economy _economy;


    private void Start()
    {
        _economy = Services.Container.Single<Economy>();
    }
}

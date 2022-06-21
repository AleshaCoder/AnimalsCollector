using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class EconomyView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private Economy _economy;

    private void Start()
    {
        _economy = Services.Container.Single<Economy>();
        _text.text = _economy.GoldCount.ToString();        
    }

    private async void OnEnable()
    {
        await Task.Delay(100);
        _economy.OnGoldChanged += (int count) => _text.text = count.ToString();
    }

    private void OnDisable()
    {
        _economy.OnGoldChanged -= (int count) => _text.text = count.ToString();
    }
}

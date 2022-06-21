using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EasySlider : MonoBehaviour
{
    [SerializeField] private Image _foreground;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _max;

    [SerializeField] private float _fillness;

    public float Fillness
    {
        get => _fillness;
        set
        {
            _fillness = value;
            if (value < 0)
                _fillness = 0;
            if (value > _max)
                _fillness = _max;
            RefreshSlider();
        }
    }

    public void SetMax(float max)
    {
        _max = max;
        RefreshSlider();
    }

    private void RefreshSlider()
    {
        _foreground.fillAmount = _fillness / _max;
        _text.text = $"{_fillness}/{_max}";
    }
}

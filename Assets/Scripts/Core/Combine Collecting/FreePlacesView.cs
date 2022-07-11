using UnityEngine;

public class FreePlacesView : MonoBehaviour
{
    [SerializeField] private EasySlider _slider;
    [SerializeField] private PlacesGroup _placesGroup;

    private void Refresh()
    {
        _slider.SetMax(_placesGroup.FreePlaceCount);
        _slider.Fillness = _placesGroup.GetTakenPlacesCount();
    }

    private void OnEnable() =>
        _placesGroup.OnFreePlacesCountChanged += Refresh;

    private void OnDisable() =>
        _placesGroup.OnFreePlacesCountChanged -= Refresh;
}

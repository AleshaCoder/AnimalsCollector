using UnityEngine;
using ECM.Walkthrough.CustomInput;

public class UpgradesTrigger : MonoBehaviour
{
    [SerializeField] private UpgradePanel _upgradePanel;
    [SerializeField] private Vector3 _rotationForParking;

    private async void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MyCharacterController movement))
        {
            if (movement.Parking)
                return;
            await movement.Park(transform.position, _rotationForParking);
            _upgradePanel.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out MyCharacterController movement))
        {
            if (movement.Parking)
                return;
            _upgradePanel.gameObject.SetActive(false);
        }
    }
}

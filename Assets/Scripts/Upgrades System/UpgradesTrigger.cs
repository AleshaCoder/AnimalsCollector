using UnityEngine;
using ECM.Walkthrough.CustomInput;
using System.Threading.Tasks;

public class UpgradesTrigger : MonoBehaviour
{
    [SerializeField] private UpgradePanel _upgradePanel;
    [SerializeField] private CameraFollower _follower;
    [SerializeField] private Joystick _joystick;
    [SerializeField] CameraFollower.CameraFollowerSettings _settings;
    [SerializeField] private Vector3 _rotationForParking;
    [SerializeField] private Vector3 _positionForParking;

    private CameraFollower.CameraFollowerSettings _oldSettings;

    private async void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MyCharacterController movement))
        {
            if (movement.Parking)
                return;
            _joystick.gameObject.SetActive(false);
            _oldSettings = (CameraFollower.CameraFollowerSettings)_follower.Settings.Clone();
            await Task.WhenAny(movement.Park(_positionForParking, _rotationForParking),
                _follower.ChangeSettings(_settings));
            _upgradePanel.gameObject.SetActive(true);
            await Task.Delay(1000);
            _joystick.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out MyCharacterController movement))
        {
            if (movement.Parking)
                return;
            _upgradePanel.gameObject.SetActive(false);
            _follower.ChangeSettings(_oldSettings);            
        }
    }
}

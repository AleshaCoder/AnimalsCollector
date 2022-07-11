using System;
using System.Threading.Tasks;
using UnityEngine;

public sealed partial class CameraFollower : MonoBehaviour, IService
{
    [Header("Common Settings")]
    [SerializeField]
    [Tooltip("ICameraPursued")]
    private MonoBehaviour _pursued;

    [SerializeField] private CameraFollowerSettings _settings;

    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Vector3 _startFollowRotation;
    private Vector3 _startFollowSpaces;
    private float _startDistance;

    public ICameraPursued Pursued
    {
        get => (ICameraPursued)_pursued;
        set => _pursued = (MonoBehaviour)value;
    }

    public CameraFollowerSettings Settings => _settings;

    public Transform TargetTransform
    {
        get
        {
            if (Pursued.TransformForFollowing == null)
            {
                _settings.Follow = false;
                Debug.LogError("Target Transform Not Set In Inspector :: CameraFollower.cs at 71");
            }
            return Pursued.TransformForFollowing;
        }
    }

    public Vector3 CameraAngle => _settings.EulerAngle;

    public float DistanceToTarget
    {
        get => _settings.DistanceToTarget;
        set => _settings.DistanceToTarget = Mathf.Max(0.0f, value);
    }

    public float FollowSpeed
    {
        get => _settings.FollowSpeed;
        set => _settings.FollowSpeed = Mathf.Max(0.0f, value);
    }

    private Vector3 CameraRelativePosition
    {
        get
        {
            Vector3 relativePosition = TargetTransform.position - transform.forward * DistanceToTarget;
            float x = _settings.FollowX ? relativePosition.x + _settings.Spaces.x : transform.position.x;
            float y = _settings.FollowY ? relativePosition.y + _settings.Spaces.y : transform.position.y;
            float z = _settings.FollowZ ? relativePosition.z + _settings.Spaces.z : transform.position.z;
            return new Vector3(x, y, z);
        }
    }

    public void Init()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;
        _startFollowRotation = _settings.EulerAngle;
        _startFollowSpaces = _settings.Spaces;
        _startDistance = DistanceToTarget;
    }

    public void MoveToStartPosition()
    {
        transform.position = _startPosition;
        transform.rotation = _startRotation;
        DistanceToTarget = _startDistance;
        _settings.EulerAngle = _startFollowRotation;
        _settings.Spaces = _startFollowSpaces;
    }

    public async Task ChangeSettings(CameraFollowerSettings newSettings)
    {
        _settings.Follow = false;
        _settings.FollowSpeed = newSettings.FollowSpeed;
        _settings.DistanceToTarget = newSettings.DistanceToTarget;
        await Task.WhenAll(ChangeSpaces(newSettings.Spaces), ChangeRotation(newSettings.EulerAngle));
        _settings = (CameraFollowerSettings)newSettings.Clone();
    }

    private async Task ChangeRotation(Vector3 eulerAngle)
    {
        while (Vector3.Distance(eulerAngle, _settings.EulerAngle) > 0.1f)
        {
            _settings.EulerAngle = Vector3.Lerp(_settings.EulerAngle, eulerAngle, FollowSpeed * Time.deltaTime);
            transform.localEulerAngles = CameraAngle;
            await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        }
    }

    private async Task ChangeSpaces(Vector3 spaces)
    {
        while (Vector3.Distance(spaces, _settings.Spaces) > 0.1f)
        {
            _settings.Spaces = Vector3.Lerp(_settings.Spaces, spaces, FollowSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, CameraRelativePosition, FollowSpeed * Time.deltaTime);
            await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        }
    }

    public void Follow(bool x = true, bool y = true, bool z = true, bool useRotation = true, float distance = -1)
    {
        _settings.FollowX = x;
        _settings.FollowY = y;
        _settings.FollowZ = z;
        _settings.UseRotation = useRotation;

        if (distance == -1)
        {
            Follow();
            return;
        }

        DistanceToTarget = distance;
        Follow();
    }

    public void Follow()
    {
        if (_settings.AutoDistance == true)
            DistanceToTarget = Vector3.Distance(transform.position, Pursued.TransformForFollowing.position);

        transform.position = CameraRelativePosition;
        _settings.Follow = true;
    }

    public void StopFollowing() => _settings.Follow = false;

    public void OnValidate()
    {
        DistanceToTarget = _settings.DistanceToTarget;
        FollowSpeed = _settings.FollowSpeed;

        if (_pursued is ICameraPursued)
        {
            Pursued = (ICameraPursued)_pursued;
        }
        else
        {
            Debug.LogError(_pursued.name + " needs to implement " + nameof(ICameraPursued));
            _pursued = null;
        }
    }

    public void FixedUpdate()
    {
        if (_settings.Physical)
            Move();
    }

    private void Move()
    {
        if (_settings.Follow == false)
            return;

        transform.position = Vector3.Lerp(transform.position, CameraRelativePosition, FollowSpeed * Time.deltaTime);

        if (_settings.UseRotation == false)
            return;

        transform.localEulerAngles = CameraAngle;
    }

    public void Update()
    {
        if (_settings.Physical == false)
            Move();
    }
}

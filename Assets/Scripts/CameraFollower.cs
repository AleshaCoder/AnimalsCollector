using UnityEngine;

public sealed class CameraFollower : MonoBehaviour
{
    [Header("Common Settings")]
    [SerializeField][Tooltip("ICameraPursued")]
    private MonoBehaviour _pursued;

    [SerializeField]
    private float _distanceToTarget = 15.0f;

    [SerializeField]
    private float _followSpeed = 3.0f;

    [Space]
    [Header("Rotation Settings")]
    [SerializeField]
    private float _cameraXRotation = 70;

    [SerializeField]
    private float _cameraYRotation = 0;

    [SerializeField]
    private float _cameraZRotation = 0;

    [Space]
    [Header("Spaces Settings")]
    [SerializeField]
    private float _cameraXSpace = 0;

    [SerializeField]
    private float _cameraYSpace = 0;

    [SerializeField]
    private float _cameraZSpace = 0;

    [Space]
    [Header("Axis Settings For Following")]
    [SerializeField]
    private bool _followX;

    [SerializeField]
    private bool _followY;

    [SerializeField]
    private bool _followZ;

    [Space]
    [Header("Follow or No in Game")]
    [SerializeField] private bool _follow = false;

    [Space]
    [Header("Use Rotation or No in Game")]
    [SerializeField] private bool _useRotation = false;

    [Space]
    [Header("Use Auto Distance or No in Game")]
    [SerializeField] private bool _autoDistance = false;

    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private Vector3 _startFollowRotation;
    private Vector3 _startFollowSpaces;
    private float _startDistance;

    public ICameraPursued Pursued
    {
        get
        {
            return (ICameraPursued)_pursued;
        }
        set
        {
            _pursued = (MonoBehaviour)value;
        }
    }

    public Transform TargetTransform
    {
        get
        {
            if (Pursued.TransformForFollowing == null)
            {
                _follow = false;
                Debug.LogError("Target Transform Not Set In Inspector :: CameraFollower.cs at 71");
            }
            return Pursued.TransformForFollowing;
        }
    }

    public Vector3 CameraAngle
    {
        get { return new Vector3(_cameraXRotation, _cameraYRotation, _cameraZRotation); }
    }

    public float DistanceToTarget
    {
        get { return _distanceToTarget; }
        set { _distanceToTarget = Mathf.Max(0.0f, value); }
    }

    public float FollowSpeed
    {
        get { return _followSpeed; }
        set { _followSpeed = Mathf.Max(0.0f, value); }
    }

    private Vector3 _cameraRelativePosition
    {
        get
        {
            Vector3 relativePosition = TargetTransform.position - transform.forward * DistanceToTarget;
            float x = (_followX == true) ? relativePosition.x + _cameraXSpace : transform.position.x;
            float y = (_followY == true) ? relativePosition.y + _cameraYSpace : transform.position.y;
            float z = (_followZ == true) ? relativePosition.z + _cameraZSpace : transform.position.z;
            return new Vector3(x, y, z);
        }
    }

    public void Init()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;
        _startFollowRotation = new Vector3(_cameraXRotation, _cameraYRotation, _cameraZRotation);
        _startFollowSpaces = new Vector3(_cameraXSpace, _cameraYSpace, _cameraZSpace);
        _startDistance = _distanceToTarget;
    }

    public void MoveToStartPosition()
    {
        transform.position = _startPosition;
        transform.rotation = _startRotation;
        _distanceToTarget = _startDistance;
        _cameraXRotation = _startFollowRotation.x;
        _cameraYRotation = _startFollowRotation.y;
        _cameraZRotation = _startFollowRotation.z;
        _cameraXSpace = _startFollowSpaces.x;
        _cameraYSpace = _startFollowSpaces.y;
        _cameraZSpace = _startFollowSpaces.z;
    }

    public void Follow(bool x = true, bool y = true, bool z = true, bool useRotation = true, float distance = -1)
    {
        _followX = x;
        _followY = y;
        _followZ = z;
        _useRotation = useRotation;
        if (distance == -1)
        {
            Follow();
            return;
        }
        _distanceToTarget = distance;
        Follow();
    }

    public void SetRotation(float x, float y, float z)
    {
        _cameraXRotation = x;
        _cameraYRotation = y;
        _cameraZRotation = z;
    }

    public void SetSpaces(float x, float y, float z)
    {
        _cameraXSpace = x;
        _cameraYSpace = y;
        _cameraZSpace = z;
    }

    public void Follow()
    {
        if (_autoDistance == true)
        {
            DistanceToTarget = Vector3.Distance(transform.position, Pursued.TransformForFollowing.position);
        }
        transform.position = _cameraRelativePosition;
        _follow = true;
    }

    public void StopFollowing()
    {
        _follow = false;
    }

    public void OnValidate()
    {
        DistanceToTarget = _distanceToTarget;
        FollowSpeed = _followSpeed;

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
        if (_follow == false)
            return;

        transform.position = Vector3.Lerp(transform.position, _cameraRelativePosition, FollowSpeed * Time.deltaTime);

        if (_useRotation == false)
            return;

        transform.localEulerAngles = CameraAngle;
    }
}

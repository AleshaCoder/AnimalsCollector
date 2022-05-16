using System.Collections;
using UnityEngine;

namespace ECM.Examples
{
    public sealed class FollowCameraController : MonoBehaviour
    {
        [SerializeField]
        private Transform _targetTransform;

        [SerializeField]
        private float _distanceToTarget = 15.0f;

        [SerializeField]
        private float _followSpeed = 3.0f;

        [SerializeField]
        private float _cameraXRotation = 70;

        [SerializeField]
        private bool _followX, _followY, _followZ;

        private IEnumerator _enumerator;

        public Transform TargetTransform
        {
            get { return _targetTransform; }
            set { _targetTransform = value; }
        }

        public Vector3 CameraAngle
        {
            get { return new Vector3(_cameraXRotation, 0, 0); }
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
            get { return TargetTransform.position - transform.forward * DistanceToTarget; }
        }

        public void ChangeRotation(float x)
        {
            if (_enumerator != null)
                StopCoroutine(_enumerator);
            _enumerator = ChangeRotationIE(x);
            StartCoroutine(_enumerator);
        }
        public IEnumerator ChangeRotationIE(float x)
        {
            float delta = x - transform.localEulerAngles.x;
            var endTime = new WaitForEndOfFrame();
            while (Mathf.Abs(transform.localEulerAngles.x - x) > Mathf.Abs(0.5f))
            {
                _cameraXRotation += delta * Time.deltaTime;
                yield return endTime;
            }
            _cameraXRotation = x;
        }

        public void OnValidate()
        {
            DistanceToTarget = _distanceToTarget;
            FollowSpeed = _followSpeed;
        }

        public void Awake()
        {
            transform.position = _cameraRelativePosition;
        }

        public void LateUpdate()
        {
            float x = (_followX == true) ? _cameraRelativePosition.x : transform.position.x;
            float y = (_followY == true) ? _cameraRelativePosition.y : transform.position.y;
            float z = (_followZ == true) ? _cameraRelativePosition.z : transform.position.z;
            transform.position = Vector3.Lerp(transform.position, new Vector3(x,y,z), FollowSpeed * Time.deltaTime);
            transform.localEulerAngles = CameraAngle;
        }
    }
}
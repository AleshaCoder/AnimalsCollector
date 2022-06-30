using ECM.Controllers;
using System;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public interface ICameraPursued
{
    Transform TransformForFollowing
    {
        get;
    }
}

namespace ECM.Walkthrough.CustomInput
{
    /// <summary>
    /// Example of a custom character controller.
    ///
    /// This show how to create a custom character controller extending one of the included 'Base' controller.
    /// In this example, we override the HandleInput method to add our custom input code.
    /// </summary>

    public class MyCharacterController : BaseCharacterController, ICameraPursued
    {
        [SerializeField] private float _maxAngleForMovement = 15;
        [SerializeField] private Transform _transformForFollowingWhenMove;
        [SerializeField] private Joystick _joystick;

        private bool _rotationCompleted;
        private bool _parking;
        public bool Parking => _parking;

        public Transform TransformForFollowing
        {
            get
            {
                if (moveDirection == Vector3.zero)
                    return transform;
                else
                    return _transformForFollowingWhenMove;
            }
        }

        public async Task Park(Vector3 position, Vector3 eulerAngles)
        {
            _parking = true;
            moveDirection = Vector3.zero;            
            var animMove = movement.transform.DOLocalMove(new Vector3(position.x, movement.transform.localPosition.y, position.z), 0.2f);
            var animRotation = movement.transform.DORotate(new Vector3(0, -90, 0), 0.5f, RotateMode.Fast);
            animMove.Play();
            await animMove.AsyncWaitForCompletion();
            animRotation.Play();
            await animRotation.AsyncWaitForCompletion();
            _parking = false;
        }

        private void Rotate()
        {
            if (moveDirection == Vector3.zero)
                return;
            var targetRotation = Quaternion.LookRotation(moveDirection, transform.up);
            var angle = Mathf.Abs(Quaternion.Angle(targetRotation, movement.rotation));
            _rotationCompleted = angle < _maxAngleForMovement;
        }

        protected override void Move()
        {
            Rotate();
            if (_rotationCompleted == false)
                moveDirection = Vector3.zero;
            base.Move();
        }

        protected override void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.P))
                pause = !pause;

            if (pause)
            {
                return;
            }

            if (Parking)
                return;

            moveDirection = new Vector3
            {
                x = -_joystick.Vertical,
                y = 0.0f,
                z = _joystick.Horizontal
            };
        }
    }
}
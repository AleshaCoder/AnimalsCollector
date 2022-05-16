using ECM.Controllers;
using UnityEngine;

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

        private bool CheckRotation()
        {
            if (moveDirection == Vector3.zero)
                return false;
            var targetRotation = Quaternion.LookRotation(moveDirection, transform.up);
            var angle = Mathf.Abs(Quaternion.Angle(targetRotation, movement.rotation));
            if (angle < _maxAngleForMovement)
                return true;
            else
                return false;
        }

        protected override void Move()
        {
            if (CheckRotation() == false)
                moveDirection = Vector3.zero;
            base.Move();
        }

        protected override void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.P))
                pause = !pause;

            moveDirection = new Vector3
            {
                x = -_joystick.Vertical,
                y = 0.0f,
                z = _joystick.Horizontal
            };
        }
    }
}

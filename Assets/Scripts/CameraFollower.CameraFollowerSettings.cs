using System;
using UnityEngine;

public sealed partial class CameraFollower
{
    [System.Serializable]
    public class CameraFollowerSettings : ICloneable
    {
        public bool Physical = true;
        public float DistanceToTarget = 15.0f;
        public float FollowSpeed = 3.0f;
        public Vector3 EulerAngle = new Vector3(70, 0, 0);
        public Vector3 Spaces = Vector3.zero;

        [Space]
        public bool FollowX;
        public bool FollowY;
        public bool FollowZ;
        public bool Follow = false;

        [Space]
        public bool UseRotation = false;
        public bool AutoDistance = false;

        public object Clone() => MemberwiseClone();
    }
}

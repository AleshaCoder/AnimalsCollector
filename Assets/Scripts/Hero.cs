using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour, ICameraPursued, IService
{
    public Transform TransformForFollowing => transform;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero :MonoBehaviour ,ICameraPursued
{
    public Transform TransformForFollowing => transform;
}

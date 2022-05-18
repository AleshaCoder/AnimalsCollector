using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    bool ReadyToPick{get; set;}
   void Pick(Transform targetTransform);
}

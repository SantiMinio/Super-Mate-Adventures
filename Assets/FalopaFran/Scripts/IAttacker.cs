using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker
{
    Vector3 GetPosition();
    float GetDamage();

    Transform GetTransform();
}

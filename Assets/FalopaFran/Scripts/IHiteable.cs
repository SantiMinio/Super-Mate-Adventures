using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHiteable
{
    public void Hit(IAttacker attacker);
}

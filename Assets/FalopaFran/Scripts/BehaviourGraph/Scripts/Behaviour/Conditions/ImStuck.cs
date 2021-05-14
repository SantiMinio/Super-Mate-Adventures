using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    [CreateAssetMenu(menuName = "Conditions/IsUnitMoving")]
    public class ImStuck : Condition
    {
        public override bool CheckCondition(StateManager state)
        {
            return !state.GetEntity.GetMovementHandler.UnitIsMoving;
        }
    }    
}

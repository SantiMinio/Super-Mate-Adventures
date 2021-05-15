using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    [CreateAssetMenu(menuName = "Conditions/CantReachDestination")]
    public class CantReachDestination : Condition
    {
        public override bool CheckCondition(StateManager state)
        {
            return state.GetEntity.GetMovementHandler.CantReachToDesiredPosition;
        }
    }    
}


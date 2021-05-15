using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    [CreateAssetMenu(menuName = "Conditions/StuckWithoutMoving")]
    public class StuckWithoutMoving : Condition
    {
        public override bool CheckCondition(StateManager state)
        {
            return state.GetEntity.GetMovementHandler.forceMove;
        }
    }    
}
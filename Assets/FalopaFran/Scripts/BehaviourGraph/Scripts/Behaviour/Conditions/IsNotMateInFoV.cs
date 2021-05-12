using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Frano
{
    [CreateAssetMenu(menuName = "Conditions/IsNotMateInFoV")]
    public class IsNotMateInFoV : Condition
    {
        public override bool CheckCondition(StateManager state)
        {
            if (!state.GetEntity.GetFoV.visibletargets.Any()) return true;
            
            return false;
        }
    }    
}


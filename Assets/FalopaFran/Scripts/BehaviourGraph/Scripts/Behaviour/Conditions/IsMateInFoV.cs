using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Frano
{
    [CreateAssetMenu(menuName = "Conditions/IsMateInFoV")]
    public class IsMateInFoV : Condition
    {
        public override bool CheckCondition(StateManager state)
        {
            if (!state.GetEntity.GetFoV.visibletargets.Any()) return false;
            
            return true;
        }
    }    
}


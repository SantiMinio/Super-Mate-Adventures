using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    [CreateAssetMenu(menuName = "Conditions/Recovered")]
    public class Recovered : Condition
    {
        public override bool CheckCondition(StateManager state)
        {
            return !state.GetEntity.GetDamaged;
        }
    }    
}

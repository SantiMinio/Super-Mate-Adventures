using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    [CreateAssetMenu(menuName = "Conditions/HaveToExplode")]
    public class HaveToExplode : Condition
    {
        public override bool CheckCondition(StateManager state)
        {
            return state.GetComponent<BombitaPolvorita>().CanExplode;
        }
    }    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    [CreateAssetMenu(menuName = "Conditions/IsNotMateInMeleeRange")]
    public class IsNotMateInMeleeRange : Condition
    {
        public override bool CheckCondition(StateManager state)
        {
            var mate = Main.instance.GetMainCharacter;
            var myEnemy = state.GetEntity.transform.position;
            
            //Debug.Log(Vector3.Distance(mate.GetPosition(), myEnemy));
            
            if (Vector3.Distance(mate.GetPosition(), myEnemy) <= state.GetEntity.meleeDistance) return false;

            return true;
        }
    }    
}


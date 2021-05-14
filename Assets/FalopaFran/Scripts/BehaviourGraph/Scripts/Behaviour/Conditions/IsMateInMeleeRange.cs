using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    [CreateAssetMenu(menuName = "Conditions/IsMateInMeleeRange")]
    public class IsMateInMeleeRange : Condition
    {
        public override bool CheckCondition(StateManager state)
        {
            var mate = Main.instance.GetMainCharacter;
            var myEnemy = state.GetEntity.transform.position;
            
            Debug.Log(Vector3.Distance(mate.GetPosition(), myEnemy));
            
            if (Vector3.Distance(mate.GetPosition(), myEnemy) <= state.GetEntity.GetMeleeDistance) return true;

            return false;
        }
    }    
}


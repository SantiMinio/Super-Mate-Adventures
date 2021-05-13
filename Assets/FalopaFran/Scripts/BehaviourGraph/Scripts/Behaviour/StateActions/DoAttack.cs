using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    [CreateAssetMenu(menuName = "Actions/DoAttack")]
    public class DoAttack : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.GetEntity.TryToDoAttack();
        }
    }    
}


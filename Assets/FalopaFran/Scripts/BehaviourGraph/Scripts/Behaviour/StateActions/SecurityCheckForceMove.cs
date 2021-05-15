using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    [CreateAssetMenu(menuName = "Actions/SecurityCheckForceMove")]
    public class SecurityCheckForceMove : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.GetEntity.GetMovementHandler.CountDownToForceMove();
        }
    }    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    [CreateAssetMenu(menuName = "Actions/RefreshTargetPosition")]
    public class RefreshTargetPosition : StateActions
    {
        public StateActions gotoTarget;
        public override void Execute(StateManager states)
        {
            gotoTarget.Execute(states);
        }
    }    
}
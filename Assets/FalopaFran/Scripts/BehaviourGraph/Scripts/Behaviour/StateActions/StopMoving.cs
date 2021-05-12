using System;
using System.Collections;
using System.Collections.Generic;
using DevTools.NodeMapping.Scripts;
using UnityEngine;

namespace Frano
{
    [CreateAssetMenu(menuName = "Actions/StopMoving")]
    public class StopMoving : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.GetEntity.GetMovementHandler.Stop();
        } 
        
    }
}
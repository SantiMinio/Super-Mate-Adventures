using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    [CreateAssetMenu(menuName = "Actions/Explode")]
    public class Explode : StateActions
    {
        public override void Execute(StateManager states)
        {
            states.GetComponent<BombitaPolvorita>().Explode();
        }
    }    
}
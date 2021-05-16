using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    [CreateAssetMenu(menuName = "Actions/CountTimeExplotion")]
    public class CountTimeExplotion : StateActions
    {
        
        public override void Execute(StateManager states)
        {
            var bombita = states.GetComponent<BombitaPolvorita>();
            
            bombita.WaitToBomb();
        }
    }    
}
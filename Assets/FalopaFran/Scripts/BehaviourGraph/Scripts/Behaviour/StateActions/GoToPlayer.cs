using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    [CreateAssetMenu(menuName = "Actions/GoToPlayer")]
    public class GoToPlayer : StateActions
    {
        
        public override void Execute(StateManager states)
        {
            var movement = states.GetEntity.GetMovementHandler;
            CharacterHead c = Main.instance.GetMainCharacter;
            
            //Debug.Log(c.GetPosition());

            if (movement.UnitIsMoving) return;
            
            movement.GoTo(c.GetPosition());
        }
    }    
}


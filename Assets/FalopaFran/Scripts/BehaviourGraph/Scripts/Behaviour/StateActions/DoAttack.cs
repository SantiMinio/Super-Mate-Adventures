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
            var player = Main.instance.GetMainCharacter;

            var dir = (player.GetPosition() - states.GetEntity.transform.position).normalized;
            states.GetEntity.transform.forward = dir;
            states.GetEntity.GetAnimator.Play("attack");
        }
    }    
}


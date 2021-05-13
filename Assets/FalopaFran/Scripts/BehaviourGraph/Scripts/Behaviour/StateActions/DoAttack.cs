using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    [CreateAssetMenu(menuName = "Actions/DoAttack")]
    public class DoAttack : StateActions
    {
        private float _count;
        [SerializeField] private float delayToAttack;
        
        public override void Execute(StateManager states)
        {
            _count += Time.deltaTime;

            if (_count < delayToAttack) return;

            _count = 0;
            var player = Main.instance.GetMainCharacter;

            var dir = (player.GetPosition() - states.GetEntity.transform.position).normalized;
            states.GetEntity.transform.forward = dir;
            states.GetEntity.GetAnimator.Play("attack");
            
            
        }
    }    
}


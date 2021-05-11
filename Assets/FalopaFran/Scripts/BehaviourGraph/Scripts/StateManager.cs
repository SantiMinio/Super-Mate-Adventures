using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    [System.Serializable]
    public class StateManager : MonoBehaviour
    {
        private EnemyDummy _myEnemy;
        public EnemyDummy GetEntity => _myEnemy;
        
        public State defaultState;
        
        public State currentState;


        public void Reset()
        {
            currentState = defaultState;
        }

        public void Init(EnemyDummy myEnemy)
        {
            _myEnemy = myEnemy;
            currentState = defaultState;
        }

        public void OnUpdate()
        {
            if(currentState != null)
            {
                currentState.Tick(this);
            }
        }
     
        public void OnFixedUpdate()
        {
            if(currentState != null)
            {
                currentState.FixedTick(this);
            }
        }
        
    }
}



using System;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    [CreateAssetMenu(menuName = "Conditions/Holder")]
    public class ConditionHolder : Condition
    {
        public List<Condition> conditions = new List<Condition>();


        public float count;
        public float timeToCheck = 1;

        private void OnEnable()
        {
            count = 0;
        }

        public override bool CheckCondition(StateManager state)
        {
            count += Time.deltaTime;

            if (count >= timeToCheck)
            {
                count = 0;
                for (int i = 0; i < conditions.Count; i++)
                {
                
                    if (conditions[i].CheckCondition(state))
                    {
                    
                    }
                    else
                    {
                        return false;
                    }
                }

                
                return true;    
            }

            return false;

        }
    }
}
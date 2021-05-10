using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    public class CharacterHead : MonoBehaviour, IAttacker
    {
        private CharController _characterController;
        private AnimEvent _animEvent;
        private Animator _animator;
        private FieldOfView _fieldOfView;

        [SerializeField] private float attackDamage;

        private void Awake()
        {
            _characterController = GetComponent<CharController>();
            _animEvent = GetComponentInChildren<AnimEvent>();
            _animator = GetComponentInChildren<Animator>();
            _fieldOfView = GetComponentInChildren<FieldOfView>();
        }

        private void Start()
        {
            _animEvent.Add_Callback("Attack", HitCloseEnemiesWithBaseAttack);
        }

        private void HitCloseEnemiesWithBaseAttack()
        {
            Debug.Log("le pego a los cercanos");
            List<Transform> targets = _fieldOfView.GetVisibleTargets;

            for (int i = 0; i < targets.Count; i++)
            {
                IHiteable hiteable = GetHiteableFromTransform(targets[i]);
                
                if(hiteable != null) hiteable.Hit(this);
            }
        }

        private void Update()
        {
            _animator.SetBool("Moving", _characterController.Moving);
        }

        public void BaseAttack()
        {
            _animator.Play("Attack");
        }

        public Vector3 GetPosition() => transform.position;
        public float GetDamage() => attackDamage;

        #region AuxMethods

        IHiteable GetHiteableFromTransform(Transform transform)
        {
            foreach (var component in transform.GetComponents<MonoBehaviour>())
            {
                if (component is IHiteable)
                {
                    return component as IHiteable;
                }
            }
            return null;
        }

        #endregion
        
    }
    
    
}


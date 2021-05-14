using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frano
{
    public class CharacterHead : MonoBehaviour, IAttacker, IHiteable
    {
        private CharController _characterController;
        private AnimEvent _animEvent;
        private Animator _animator;
        private FieldOfView _fieldOfView;
        private LifeHandler _lifeHandler;


        public ManaSystem manaSystem = new ManaSystem();
        [SerializeField] private ParticleSystem takeDamageFeedback;
        
        [SerializeField] private float attackDamage;

        
        
        float initDamage;

        private void Awake()
        {
            _characterController = GetComponent<CharController>();
            _animEvent = GetComponentInChildren<AnimEvent>();
            _animator = GetComponentInChildren<Animator>();
            _fieldOfView = GetComponentInChildren<FieldOfView>();
            
            _lifeHandler = GetComponent<LifeHandler>();
            _lifeHandler.RefreshLifePercent += UIManager.instance.ChangeLifeBar;
            initDamage = attackDamage;
        }

        private void Start()
        {
            _lifeHandler.OnDead += Dead;
            _animEvent.Add_Callback("Attack", HitCloseEnemiesWithBaseAttack);
            manaSystem.Initialize();
            
            Main.instance.EventManager.SubscribeToEvent(GameEvent.UseCard, CardUsed);
            
        }

        private void Dead()
        {
            //_animator.Play("Dead");
            Main.instance.EventManager.TriggerEvent(GameEvent.MateDead);
        }

        private void CardUsed()
        {
            _animator.Play("UseCard");
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
        public LifeHandler GetLifeHandler => _lifeHandler; 
        public void AddDamage(float dmg) => attackDamage += dmg;
        public bool IsDamageBuffed() => attackDamage == initDamage ? false : true;
        public void ResetDamageValue() => attackDamage = initDamage;

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

        public void Hit(IAttacker attacker)
        {
            if (_lifeHandler.Invulnerability) return;
            _lifeHandler.TakeDamage(attacker.GetDamage());
            _animator.Play("Take damage");
            
            takeDamageFeedback.transform.forward = (transform.position - attacker.GetPosition()).normalized;
            takeDamageFeedback.Play();
        }
    }
    
    
}


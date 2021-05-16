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


        private bool baseAttackOnCD = false;
        [SerializeField] private float baseAttackCD;
        private CDModule baseAttackCDTimer = new CDModule();

        public ManaSystem manaSystem = new ManaSystem();
        [SerializeField] private ParticleSystem takeDamageFeedback;
        
        [SerializeField] private float attackDamage;
        [SerializeField] AudioClip deadSound = null;
        
        
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
            AudioManager.instance.GetSoundPool(deadSound.name, AudioManager.SoundDimesion.ThreeD, deadSound);
        }

        private void Dead()
        {
            //_animator.Play("Dead");
            AudioManager.instance.PlaySound(deadSound.name);
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
            
            baseAttackCDTimer.UpdateCD();
        }

        public void BaseAttack()
        {
            if (baseAttackOnCD) return;

            baseAttackOnCD = true;
            baseAttackCDTimer.AddCD("baseAttack_CD", ResetBaseAttackCD, baseAttackCD);
            _animator.Play("Attack");

        }

        void ResetBaseAttackCD()
        {
            baseAttackOnCD = false;
        }

        public Vector3 GetPosition() => transform.position;
        public float GetDamage() => attackDamage;
        public Transform GetTransform()
        {
            return transform;
        }

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


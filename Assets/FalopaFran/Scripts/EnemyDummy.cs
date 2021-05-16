using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Frano;
using UnityEngine;

public class EnemyDummy : MonoBehaviour, IHiteable, IAttacker
{
    [SerializeField] ParticleSystem hittedFeedback;
    private Rigidbody _rb;
    [SerializeField] private MovementHandler _movementHandler = new MovementHandler();
    [SerializeField] private FieldOfView _fieldOfView;
    [SerializeField] private FieldOfView _attackFieldOfView;
    [SerializeField] int giveScore = 50;
    [SerializeField] GameObject deadbody;

    [SerializeField] private float turnSpeed, moveSpeed;
    
    private Animator _animator;
    private AnimEvent _animEvent;
    private StateManager _fsm;
    private LifeHandler _lifeHandler;

    [SerializeField] private float attackDamage;
    public float _count;
    [SerializeField] private float delayToAttack;

    [SerializeField] private float meleeDistance;

    [SerializeField] private float knockbackIntensity;

    private CDModule takeDamageTimer = new CDModule();
    

    [SerializeField] AudioClip takeDamageSound = null;
    [SerializeField] AudioClip deadSound = null;

    public bool GetDamaged { get; private set; }
    public float GetMeleeDistance => meleeDistance;
    public MovementHandler GetMovementHandler => _movementHandler;
    public Animator GetAnimator => _animator;
    public FieldOfView GetFoV => _fieldOfView;

    public Action<EnemyDummy> ReturnToSpawn;
    void Awake()
    {
        _lifeHandler = GetComponent<LifeHandler>();

        _lifeHandler.OnDead += Dead;

        _fsm = GetComponent<StateManager>();
        _fsm.Init(this);

        _rb = GetComponent<Rigidbody>();
        //_movementHandler = new MovementHandler();
        _movementHandler.Init(FindObjectOfType<Pathfinding>(), this, _rb);
        _movementHandler.SetSpeeds(turnSpeed, moveSpeed);

        _animator = GetComponentInChildren<Animator>();
        _animEvent = GetComponentInChildren<AnimEvent>();

    }

    public void Dead()
    {
        AudioManager.instance.PlaySound(deadSound.name);
        deadbody.SetActive(true);
        deadbody.transform.parent = null;
        deadbody.transform.position = transform.position;
        deadbody.GetComponent<RagdollHandler>().PushTo(_lastDirAttack);
        ReturnToSpawn?.Invoke(this);
        ScoreSystem.instance.RefreshScore(giveScore);
        Main.instance.EventManager.TriggerEvent(GameEvent.EnemyDead);
        Destroy(gameObject);
    }

    private void Start()
    {
        _animEvent.Add_Callback("doDamage", DoAttack);

        AudioManager.instance.GetSoundPool(takeDamageSound.name, AudioManager.SoundDimesion.ThreeD, takeDamageSound);
        AudioManager.instance.GetSoundPool(deadSound.name, AudioManager.SoundDimesion.ThreeD, deadSound);

        //meleeDistance = 6.5f;
        //attackDamage = 1f;
    }

    

    private void Update()
    {
        _animator.SetBool("moving", _movementHandler.moving);
        _fsm.OnUpdate();
        takeDamageTimer.UpdateCD();
    }

    private void FixedUpdate()
    {
        _fsm.OnFixedUpdate();
        _movementHandler.OnFixedUpdate();
    }

    public void TryToDoAttack()
    {
        _count += Time.deltaTime;

        if (_count >= delayToAttack)
        {
            _count = 0;
            
            var player = Main.instance.GetMainCharacter;

            var dir = (player.GetPosition() - transform.position).normalized;
            transform.forward = dir;
            GetAnimator.Play("attack");
        }
    }

    private void DoAttack()
    {
        List<Transform> targets = _attackFieldOfView.GetVisibleTargets;

        for (int i = 0; i < targets.Count; i++)
        {
            IHiteable hiteable = GetHiteableFromTransform(targets[i]);
                
            if(hiteable != null) hiteable.Hit(this);
        }
    }

    [SerializeField]private Vector3 _lastDirAttack;
    public void Hit(IAttacker atttacker)
    {
        AudioManager.instance.PlaySound(takeDamageSound.name);
        Debug.Log("Recibo daño");

        _lifeHandler.TakeDamage(atttacker.GetDamage());
        Vector3 attackDirection = (transform.position - atttacker.GetPosition()).normalized;

        _lastDirAttack = attackDirection;
        _movementHandler.myRb.AddForce(attackDirection * knockbackIntensity, ForceMode.Impulse);

        hittedFeedback.transform.forward = attackDirection;
        hittedFeedback.Play();
        
        _animator.Play("Take Damage");
        GetDamaged = true;
        takeDamageTimer.AddCD("getDamageCD", ResetGetDamageCD, 1);
    }

    void ResetGetDamageCD()
    {
        GetDamaged = false;
    }
    
    
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

    public Vector3 GetPosition() {return transform.position; }

    public float GetDamage(){return attackDamage;}
    public Transform GetTransform()
    {
        return transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_movementHandler.currentPosToGo, 1f);
    }
}

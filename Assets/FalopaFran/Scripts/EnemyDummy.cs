using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Frano;
using UnityEngine;

public class EnemyDummy : MonoBehaviour, IHiteable
{
    [SerializeField] ParticleSystem hittedFeedback;
    private Rigidbody _rb;
    [SerializeField]private MovementHandler _movementHandler;
    [SerializeField] private FieldOfView _fieldOfView;

    
    private Animator _animator;
    private AnimEvent _animEvent;
    private StateManager _fsm;
    private LifeHandler _lifeHandler;
    
    
    public float meleeDistance { get; private set; }
    public MovementHandler GetMovementHandler => _movementHandler;
    public Animator GetAnimator => _animator;
    public FieldOfView GetFoV => _fieldOfView;
    void Awake()
    {
        _lifeHandler = GetComponent<LifeHandler>();

        _lifeHandler.OnDead += () => Destroy(gameObject);

        _fsm = GetComponent<StateManager>();
        _fsm.Init(this);

        _rb = GetComponent<Rigidbody>();
        _movementHandler = new MovementHandler();
        _movementHandler.Init(FindObjectOfType<Pathfinding>(), this, _rb);

        _animator = GetComponentInChildren<Animator>();
        _animEvent = GetComponentInChildren<AnimEvent>();

    }

    private void Start()
    {
        _animEvent.Add_Callback("attack", DoAttack);

        meleeDistance = 12f;
    }

    

    private void Update()
    {
        _animator.SetBool("moving", _movementHandler.moving);
        _fsm.OnUpdate();
    }

    private void FixedUpdate()
    {
        _fsm.OnFixedUpdate();
        _movementHandler.OnFixedUpdate();
    }

    private void DoAttack()
    {
        Debug.Log("ataque bizcocho");
    }
    
    public void Hit(IAttacker atttacker)
    {
        Debug.Log("Recibo daño");

        _lifeHandler.TakeDamage(atttacker.GetDamage());
        

        hittedFeedback.transform.forward = (transform.position - atttacker.GetPosition()).normalized;
        hittedFeedback.Play();
    }
}

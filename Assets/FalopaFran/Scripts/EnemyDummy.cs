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

    public MovementHandler GetMovementHandler => _movementHandler;
    
    public FieldOfView GetFoV => _fieldOfView;
    
    private StateManager _fsm;
    
    private LifeHandler _lifeHandler;
    void Awake()
    {
        _lifeHandler = GetComponent<LifeHandler>();

        _lifeHandler.OnDead += () => Destroy(gameObject);

        _fsm = GetComponent<StateManager>();
        _fsm.Init(this);

        _rb = GetComponent<Rigidbody>();
        _movementHandler = new MovementHandler();
        _movementHandler.Init(FindObjectOfType<Pathfinding>(), this, _rb);
        


    }
    private void Update()
    {
        _fsm.OnUpdate();
        
        // if (_fieldOfView.GetVisibleTargets.Any())
        // {
        //     Debug.Log(_fieldOfView.GetVisibleTargets[0].position);
        //     _movementHandler.GoTo(_fieldOfView.GetVisibleTargets[0].position);
        // }
    }

    private void FixedUpdate()
    {
        _fsm.OnFixedUpdate();
        _movementHandler.OnFixedUpdate();
    }

    public void Hit(IAttacker atttacker)
    {
        Debug.Log("Recibo daño");

        _lifeHandler.TakeDamage(atttacker.GetDamage());
        

        hittedFeedback.transform.forward = (transform.position - atttacker.GetPosition()).normalized;
        hittedFeedback.Play();
    }
}

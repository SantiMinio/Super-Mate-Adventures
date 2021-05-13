using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySpawner : MonoBehaviour, IHiteable
{
    private EnemyDummy _currentEnemy;

    [SerializeField] private string prefabName;
    private Animator _anim;
    private LifeHandler _lifeHandler;
    [SerializeField] private ParticleSystem _takeDamageFeedback;

    [SerializeField]
    private GameObject nodeBlocker;

    [SerializeField] private Transform spawnPoint;

    private void Awake()
    {
        
        _lifeHandler = GetComponent<LifeHandler>();
        _anim = GetComponent<Animator>();
        _lifeHandler.OnDead += Dead;
    }

    private void Start()
    {
        nodeBlocker.SetActive(false);
    }

    private void Dead()
    {
      Destroy(gameObject); 
    }

    void SpawnNewEnemy()
    {
        EnemyDummy dummy = Resources.Load<EnemyDummy>(prefabName);

        _currentEnemy = Instantiate(dummy, spawnPoint.position, spawnPoint.rotation);
    }

    private void Update()
    {
        if(_currentEnemy == null) SpawnNewEnemy();
    }

    public void Hit(IAttacker attacker)
    {
        _anim.Play("Dmg");
        _takeDamageFeedback.Play();
        _lifeHandler.TakeDamage(attacker.GetDamage());
    }
}

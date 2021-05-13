using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySpawner : MonoBehaviour, IHiteable
{
    [SerializeField] private string prefabName;
    private Animator _anim;
    private LifeHandler _lifeHandler;
    [SerializeField] private ParticleSystem _takeDamageFeedback;

    [SerializeField] private float distanceToActivate = 30f;
    
    private float _count;
    [SerializeField] private float timeBetweenSpawns;
    [SerializeField] private int amountSpawnInWave;

    [SerializeField]
    private GameObject nodeBlocker;

    [SerializeField] private List<Transform> spawnPoint;

    private List<EnemyDummy> _currentSpawnedEnemies = new List<EnemyDummy>();

    [SerializeField]private bool active;

    private void Awake()
    {
        
        _lifeHandler = GetComponent<LifeHandler>();
        _anim = GetComponent<Animator>();
        _lifeHandler.OnDead += Dead;
    }

    private void Start()
    {
        Main.instance.EventManager.SubscribeToEvent(GameEvent.StartNewWave, ActiveSpawner);
        Main.instance.EventManager.SubscribeToEvent(GameEvent.FinishWaveSpawn, DisableSpawner);
        
        nodeBlocker.SetActive(false);
        
    }

    private void ActiveSpawner()
    {
        active = true;
    }

    private void DisableSpawner()
    {
        active = false;
    }
    
    private void Dead()
    {
      Destroy(gameObject); 
    }

    void Spawns()
    {
        for (int i = 0; i < amountSpawnInWave; i++)
        {
            int spanwIndex = i;

            if (spanwIndex >= spawnPoint.Count) spanwIndex = 0 +  i - spawnPoint.Count;
                
            SpawnNewEnemy(spawnPoint[spanwIndex]);
        }
    }

    void SpawnNewEnemy(Transform spawnPoint)
    {
        
        EnemyDummy dummy = Resources.Load<EnemyDummy>(prefabName);

        var _currentEnemy = Instantiate(dummy, spawnPoint.position, spawnPoint.rotation);
        _currentSpawnedEnemies.Add(_currentEnemy);
        
        Main.instance.EventManager.TriggerEvent(GameEvent.SpawnCookie);
    }

    private void Update()
    {
        if(!active) return;
        
        
        if (Vector3.Distance(Main.instance.GetMainCharacter.GetPosition(), transform.position) >
            distanceToActivate) return;

        _count += Time.deltaTime;
        
        if (_count >= timeBetweenSpawns)
        {
            _count = 0;
            Spawns();

        }
        
    }

    public void Hit(IAttacker attacker)
    {
        _anim.Play("Dmg");
        _takeDamageFeedback.Play();
        _lifeHandler.TakeDamage(attacker.GetDamage());
    }

    private void OnDrawGizmosSelected()
    {
  
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToActivate);
        
    }
}

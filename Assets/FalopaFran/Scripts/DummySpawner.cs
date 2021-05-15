using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


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

    [SerializeField] private List<Transform>  spawnPoints;
    [SerializeField] Transform  spawnPoint;
    [SerializeField] private float spawnRadius;

    [SerializeField] private List<EnemyDummy> _currentSpawnedEnemies = new List<EnemyDummy>();

    public List<EnemyDummy> GetEnemiesSpawned() => _currentSpawnedEnemies;
    public bool active { get; private set; }


    public bool AreEnemiesSpawnedAlive()
    {
        //Debug.Log("el spawn " + gameObject.name + "  " + _currentSpawnedEnemies.Any(x => x != null));
        return _currentSpawnedEnemies.Any(x => x != null);
    }

    public int GetLiveEnemies()
    {
        return _currentSpawnedEnemies.Count;
    }
    
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
            SpawnNewEnemy();
        }
    }

    void SpawnNewEnemy()
    {
        
        EnemyDummy dummy = Resources.Load<EnemyDummy>(prefabName);

        var random = (Random.insideUnitCircle * spawnRadius);

        var newVector = new Vector3(random.x, 0, random.y); 
        
        
        Vector3 realSpawnPos = new Vector3(newVector.x, spawnPoint.localPosition.y, newVector.z);
        var _currentEnemy = Instantiate(dummy, spawnPoint.position, spawnPoint.rotation);
        _currentEnemy.transform.position = realSpawnPos + spawnPoint.position;
        _currentEnemy.ReturnToSpawn = (x) => _currentSpawnedEnemies.Remove(x);
        _currentSpawnedEnemies.Add(_currentEnemy);
        
        Main.instance.EventManager.TriggerEvent(GameEvent.SpawnCookie);
    }

    private void Update()
    {
        if(!active) return;


        _currentSpawnedEnemies = _currentSpawnedEnemies.Where(x => x != null).ToList();
        
        
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
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawnPoint.position, spawnRadius);
        
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceToActivate);
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummySpawner : MonoBehaviour
{
    private EnemyDummy _currentEnemy;
    
    void SpawnNewEnemy()
    {
        EnemyDummy dummy = Resources.Load<EnemyDummy>("EnemmyDummy");

        _currentEnemy = Instantiate(dummy, transform.position, transform.rotation);
    }

    private void Update()
    {
        if(_currentEnemy == null) SpawnNewEnemy();
    }
}

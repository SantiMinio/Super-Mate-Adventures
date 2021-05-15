using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombitaPolvorita : EnemyDummy
{
    float _countExplode;
    [SerializeField] private float timeToExplode; 
    
    public void Explode()
    {
        Debug.Log("Exploto");
    }
    
    public void WaitToBomb()
    {
        Debug.Log("Espero");
    }
}

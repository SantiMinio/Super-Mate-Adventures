using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollHandler : MonoBehaviour
{
    private Rigidbody _myRb;

    [SerializeField] private Rigidbody hip;
    
    [SerializeField] private float pushbackForce;


    private float _count;
    private bool countDown;
    [SerializeField] private float countDownToDestroy;
    void Awake()
    {
        _myRb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (countDown)
        {
            _count += Time.deltaTime;
            if (_count >= countDownToDestroy)
            {
                Destroy(gameObject);
            }    
        }
        
        
        
    }

    public void PushTo(Vector3 lastDirAttack)
    {
        //Debug.Log(lastDirAttack);

        countDown = true;
        hip.AddForce(pushbackForce * lastDirAttack, ForceMode.Impulse);   
    }
}

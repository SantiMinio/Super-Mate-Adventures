using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollHandler : MonoBehaviour
{
    private Rigidbody _myRb;

    [SerializeField] private Rigidbody hip;
    
    [SerializeField] private float pushbackForce;
    
    void Awake()
    {
        _myRb = GetComponent<Rigidbody>();
    }

    public void PushTo(Vector3 lastDirAttack)
    {
        Debug.Log(lastDirAttack);
        
        
        hip.AddForce(pushbackForce * lastDirAttack, ForceMode.Impulse);   
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    [SerializeField] private GameObject myCam;
    
    [SerializeField] private Transform maxOut;
    [SerializeField] private Transform maxIn;

    

    // Update is called once per frame
    void LateUpdate()
    {


        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Debug.Log("subo");
            myCam.transform.position += myCam.transform.forward.normalized;
    
        }
        
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Debug.Log("bajo");
            myCam.transform.position -= myCam.transform.forward.normalized;
            
        }
        
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seguidor : MonoBehaviour
{
    public GameObject punto;
    // Update is called once per frame
    void Update()
    {
        transform.position = punto.transform.position;
    }
}

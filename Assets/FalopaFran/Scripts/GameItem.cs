using System;
using System.Collections;
using System.Collections.Generic;
using Frano;
using UnityEngine;

public class GameItem : MonoBehaviour , IPickable
{

    [SerializeField] private LayerMask triggerLayer;

    [SerializeField] private float feedbackTime;

    [SerializeField] private ParticleSystem feedbackParticle;
    
    public GameObject Picker { get; private set; }
    
    private CDModule timer = new CDModule();
    private void OnTriggerEnter(Collider other)
    {
        
        if ((triggerLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            Picker = other.gameObject;
            Pick();
        }

    }

    public virtual void Pick()
    {
        GetComponent<Collider>().enabled = false;
        feedbackParticle.Play();
        timer.AddCD("feedbackTime", ()=> Destroy(gameObject), feedbackTime);
    }

    private void Update()
    {
        timer.UpdateCD();
    }
}

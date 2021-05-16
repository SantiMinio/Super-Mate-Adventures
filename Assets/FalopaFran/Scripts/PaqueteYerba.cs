using System.Collections;
using System.Collections.Generic;
using Frano;
using UnityEngine;

public class PaqueteYerba : MonoBehaviour, IHiteable
{
    [SerializeField] private float healPerHit;
    [SerializeField] private int hitsPerWave;

    [SerializeField] private ParticleSystem feedbackOnHit;

    private int _hitCount;
    
    private bool active;
    void Start()
    {
        Main.instance.EventManager.SubscribeToEvent(GameEvent.StartNewWave, YerbaOn);
        Main.instance.EventManager.SubscribeToEvent(GameEvent.KilledAllEnemiesSpawned, YerbaOff);
    }


    void YerbaOn()
    {
        active = true;
    }
    
    void YerbaOff()
    {
        active = false;
        ResetHitCount();
    }
    
    void ResetHitCount()
    {
        _hitCount = 0;
    }
    public void Hit(IAttacker attacker)
    {
        if (!active) return;
        
        if(_hitCount >= hitsPerWave) return;
        
        if (attacker is CharacterHead)
        {
            feedbackOnHit.Play();
            var character = attacker.GetTransform().GetComponent<CharacterHead>();
            character.GetLifeHandler.Heal(healPerHit);

            _hitCount++;
        }
        
    }
}

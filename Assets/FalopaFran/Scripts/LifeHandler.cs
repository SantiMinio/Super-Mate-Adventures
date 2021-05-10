using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeHandler : MonoBehaviour
{
    [SerializeField] private float maxLife;

    public float _currentLife { get; private set; }

    public event Action OnDead;
    
    private void Awake()
    {
        ResetLife();
    }

    public void TakeDamage(float value)
    {
        _currentLife -= value;

        if (_currentLife <= 0)
            Die();
    }

    public void Heal(float value)
    {
        _currentLife += value;

        if (_currentLife >= maxLife)
            _currentLife = maxLife;
    }
    
    private void Die()
    {
        OnDead?.Invoke();
    }

    public void ResetLife()
    {
        _currentLife = maxLife;
    }
}

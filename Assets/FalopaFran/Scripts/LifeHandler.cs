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

        UIManager.instance?.ChangeLifeBar(_currentLife / maxLife);
    }

    public void Heal(float value)
    {
        _currentLife += value;

        if (_currentLife >= maxLife)
            _currentLife = maxLife;
        UIManager.instance?.ChangeLifeBar(_currentLife / maxLife);
    }
    
    private void Die()
    {
        OnDead?.Invoke();
    }

    public void ResetLife()
    {
        _currentLife = maxLife;
        UIManager.instance?.ChangeLifeBar(_currentLife / maxLife);
    }

    public bool CheckEnoughLife(float life) => _currentLife - life < 0 ? false : true;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeHandler : MonoBehaviour
{
    [SerializeField] public float maxLife;

    public float _currentLife { get; private set; }

    public event Action OnDead;
    public event Action<float> RefreshLifePercent = delegate { };

    public bool Invulnerability { get; private set; }
    
    private void Start()
    {
        ResetLife();
    }

    public void TakeDamage(float value)
    {
        _currentLife -= value;

        if (_currentLife <= 0)
            Die();

        RefreshLifePercent?.Invoke(_currentLife / maxLife);
    }

    public void ChangeInvulnerability(bool b) => Invulnerability = b;

    public void Heal(float value)
    {
        _currentLife += value;

        if (_currentLife >= maxLife)
            _currentLife = maxLife;

        RefreshLifePercent?.Invoke(_currentLife / maxLife);
    }
    
    private void Die()
    {
        OnDead?.Invoke();
    }

    public void ResetLife()
    {
        _currentLife = maxLife;

        RefreshLifePercent?.Invoke(_currentLife / maxLife);
    }

    public bool CheckEnoughLife(float life) => _currentLife - life <= 0 ? false : true;

    public bool IsFullLife() => _currentLife == maxLife ? true : false;
}

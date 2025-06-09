using System;
using UnityEngine;

public class Health
{
    private int _health;
    private int _maxHealth;
    private int _minHealth;

    public event Action Expired;

    public Health(int maxHealth)
    {
        _minHealth = 0;
        _maxHealth = maxHealth;

        _health = _maxHealth;
    }

    public void DecreaseHealth(int amount)
    {
        _health -= Mathf.Abs(amount);

        if (_health <= _minHealth) 
        { 
            Expired?.Invoke();
        }
    }

    public void IncreaseHealth(int amount)
    {
        _health += Mathf.Abs(amount);

        Mathf.Clamp(_health, _minHealth, _maxHealth);
    }
}

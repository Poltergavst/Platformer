using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _maxHealth;

    private int _health;
    private int _minHealth;

    public event Action Expired;

    private void Start()
    {
        _minHealth = 0;
        _health = _maxHealth;
    }

    public int GetCurrent()
    {
        return _health;
    }

    public void Decrease(int amount)
    {
        _health -= Mathf.Abs(amount);

        Debug.Log($"HealthDecreased: {_health}");

        if (_health <= _minHealth) 
        { 
            Expired?.Invoke();
        }
    }

    public void Increase(int amount)
    {
        _health += Mathf.Abs(amount);

        _health = Mathf.Clamp(_health, _minHealth, _maxHealth);

        Debug.Log($"HealthIncreased: {_health}");
    }

    public void Reset()
    {
        _health = _maxHealth;
    }
}

using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _max;

    private float _min;

    public event Action<float> Changed;
    public event Action Reset;
    public event Action Expired;

    public float Max => _max;
    public float Min => _min;
    public float Current { get; private set; }

    private void Awake()
    {
        _min = 0;
        Current = _max;
    }

    public void Decrease(float amount)
    {
        if (amount < 0)
            return;

        ChangeCurrent(-amount);

        if (Current <= _min) 
        { 
            Expired?.Invoke();
        }
    }

    public void Increase(float amount)
    {
        if (amount < 0)
            return;

        ChangeCurrent(amount);
    }

    private void ChangeCurrent(float amount)
    {
        Current += amount;

        Current = Mathf.Clamp(Current, _min, _max);

        Changed?.Invoke(Current);
    }

    public void SetToMax()
    {
        ChangeCurrent(_max);

        Reset?.Invoke();
    }
}

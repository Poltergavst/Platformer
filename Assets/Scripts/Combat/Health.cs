using System;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _max;

    private int _min;

    public event Action Expired;

    public int Current { get; private set; }

    private void Start()
    {
        _min = 0;
        Current = _max;
    }

    public void Decrease(int amount)
    {
        if (amount < 0)
            return;

        ChangeCurrent(-amount);

        if (Current <= _min) 
        { 
            Expired?.Invoke();
        }
    }

    public void Increase(int amount)
    {
        if (amount < 0)
            return;

        ChangeCurrent(amount);
    }

    private void ChangeCurrent(int amount)
    {
        Current += amount;

        Debug.Log(Current);

        Current = Mathf.Clamp(Current, _min, _max);
    }

    public void Reset()
    {
        Current = _max;
    }
}

using System;
using UnityEngine;

public class Healthpack : MonoBehaviour, ICollectable
{
    public int HealAmount { get; private set; }

    public static event Action<Healthpack> Collected;

    private void Awake()
    {
        HealAmount = 1;
    }

    public void Collect()
    {
        Collected?.Invoke(this);
    }
}

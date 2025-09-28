using System;
using UnityEngine;

public class Healthpack : MonoBehaviour, ICollectable
{
    public static event Action<Healthpack> Collected;

    public int HealAmount { get; private set; }

    private void Awake()
    {
        HealAmount = 1;
    }

    public void Collect(Collector collector)
    {
        collector.gameObject.TryGetComponent<Health>(out Health health);

        health.Increase(HealAmount);

        Collected?.Invoke(this);
    }
}

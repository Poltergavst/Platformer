using System;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{
    [SerializeField] CoinSpawner coinspawner;
    [SerializeField] HealthpackSpawner healthpackSpawner;

    public static event Action<Coin> Collected;

    public void Collect(Collector collector)
    {
        Collected?.Invoke(this);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{
    [SerializeField] CoinSpawner coinspawner;
    [SerializeField] HealthpackSpawner healthpackSpawner;

    public static event Action<Coin> Collected;

    public void Collect()
    {
        Collected?.Invoke(this);
    }
}

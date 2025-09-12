using System;
using UnityEngine;

public class Coin : MonoBehaviour, ICollectable
{
    public static event Action<Coin> Collected;

    public void Collect(Collector collector)
    {
        Collected?.Invoke(this);
    }
}

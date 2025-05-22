using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public event Action<Coin> Collected;

    public void Collect()
    {
        Collected?.Invoke(this);
    }
}

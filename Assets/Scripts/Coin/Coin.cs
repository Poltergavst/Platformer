using System;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public event Action<Coin> CoinCollected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out _))
        {
            Collect();
        }
    }

    private void Collect()
    {
        CoinCollected?.Invoke(this);
    }
}

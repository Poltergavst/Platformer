using UnityEngine;

public class CoinSpawner : CollectableSpawner<Coin>
{
    private void OnEnable()
    {
        Coin.Collected += Despawn;
    }

    private void OnDisable()
    {
        Coin.Collected -= Despawn;
    }
}

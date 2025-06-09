using UnityEngine;

public class HealthpackSpawner : CollectableSpawner<Healthpack>
{
    private void OnEnable()
    {
        Healthpack.Collected += Despawn;
    }

    private void OnDisable()
    {
        Healthpack.Collected -= Despawn;
    }
}

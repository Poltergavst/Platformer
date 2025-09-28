using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public event Action<IDamagable> CollidedWithDamagable;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            CollidedWithDamagable?.Invoke(damagable);
        }
    }
}

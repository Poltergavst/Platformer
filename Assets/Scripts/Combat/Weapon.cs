using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Transform _weaponObject;

    public event Action<IDamagable> CollidedWithDamagable;

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent(out IDamagable damagable))
        {
            CollidedWithDamagable?.Invoke(damagable);
        }
    }

    public Vector3 GetPosition()
    {
        return _weaponObject.transform.position;
    }
}

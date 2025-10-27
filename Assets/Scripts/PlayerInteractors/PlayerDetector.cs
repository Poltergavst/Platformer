using System;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public event Action<Player> PlayerDetected;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryDetectPlayer(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        TryDetectPlayer(collider.gameObject);
    }

    private void TryDetectPlayer(GameObject collidedObject)
    {
        if (collidedObject.TryGetComponent(out Player player))
        {
            PlayerDetected?.Invoke(player);
        }
    }
}

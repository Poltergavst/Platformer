using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ICollectable item))
        {
            item.Collect(this);
        }
    }
}

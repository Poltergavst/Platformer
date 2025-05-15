using UnityEngine;

public class LevelBounds: MonoBehaviour
{
    [SerializeField] private Collider2D _boundingCollider;

    public Vector2 MaxBounds { get; private set; }
    public Vector2 MinBounds { get; private set; }

    private void Awake()
    {
        GetBounds();
    }

    public void GetBounds()
    {
        MaxBounds = _boundingCollider.bounds.max;
        MinBounds = _boundingCollider.bounds.min;
    }
}

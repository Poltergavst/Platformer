using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
[RequireComponent(typeof(Rotator))]
public abstract class Mover : MonoBehaviour
{
    [SerializeField] protected float Speed;

    protected Rotator Rotator;
    protected Rigidbody2D Rigidbody;

    protected virtual void Awake()
    {
        Rotator = GetComponent<Rotator>();
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    public abstract void Move();

    public Vector2 GetDirection()
    {
        return Rotator.IsFacingRight ? Vector2.right : Vector2.left;
    }
}

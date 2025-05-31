using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
[RequireComponent(typeof(Rotator), typeof(GroundChecker))]
public abstract class Mover : MonoBehaviour
{
    [SerializeField] protected float Speed;

    protected Rotator Rotator;
    protected Rigidbody2D Rigidbody;
    protected GroundChecker GroundChecker;

    protected virtual void Awake()
    {
        Rotator = GetComponent<Rotator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        GroundChecker = GetComponent<GroundChecker>();
    }

    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected abstract void Move();
}

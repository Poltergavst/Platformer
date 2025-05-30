using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
[RequireComponent(typeof(Rotator), typeof(GroundChecker))]
public abstract class Mover : MonoBehaviour
{
    [SerializeField] protected float _speed;

    protected Rotator _rotator;
    protected Rigidbody2D _rigidbody;
    protected GroundChecker _groundChecker;

    protected virtual void Awake()
    {
        _rotator = GetComponent<Rotator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundChecker = GetComponent<GroundChecker>();
    }

    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected abstract void Move();
}
